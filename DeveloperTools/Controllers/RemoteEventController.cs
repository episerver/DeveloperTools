using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Shell.Gadgets;
using System.Web.Mvc;
using EPiServer.Framework.Initialization;
using EPiServer.Shell.Web;
using EPiServer.ServiceLocation;
using EPiServer.Events.Remote;
using System.Collections;
using DeveloperTools.Models;
using EPiServer.Events.Clients;
using EPiServer.Events.Providers;
using System.Threading.Tasks;
using EPiServer.Licensing.Services;

namespace DeveloperTools.Controllers
{
    public class RemoteEventController : DeveloperToolsController
    {
        private static Guid RaiserId = Guid.NewGuid();
        private readonly IEventRegistry _eventRegistry;
        private readonly ServerStateService _serverStateService;
        private readonly EventProviderService _providerService;

        public RemoteEventController(IEventRegistry eventRegistry, ServerStateService serverStateService, EventProviderService providerService)
        {
            _eventRegistry = eventRegistry;
            _serverStateService = serverStateService;
            _providerService = providerService;
        }

        public ActionResult Index()
        {
            return View("Index", GetRemoteEventModel());
        }

        [HttpPost, ActionName("Index")]
        public ActionResult SendMessage(SendRemoteEventModel sendRemoteEventModel)
        {
            int i = 0;
            if (sendRemoteEventModel.EventId != Guid.Empty)
            {
                Event e = Event.Get(sendRemoteEventModel.EventId);
                for (; i < sendRemoteEventModel.NumberOfevents; i++)
                {
                    Task.Factory.StartNew(() => e.Raise(RaiserId, sendRemoteEventModel.Param));
                }
                sendRemoteEventModel.NumberOfeventsSent = i;
            }
            else
            {
                sendRemoteEventModel.NumberOfeventsSent = i;
            }
            RemoteEventsModel model = GetRemoteEventModel();
            model.SendRemoteEventModel = sendRemoteEventModel;
            return View("Index", model);
        }

        private RemoteEventsModel GetRemoteEventModel()
        {
            List<RemoteEventModel> remoteEventModel = new List<RemoteEventModel>();
            long sentEvents = 0;
            long receivedEvents = 0;
            foreach (var ev in _eventRegistry.List())
            {
                var rm = new RemoteEventModel() { Name = ev.Id.ToString(), NumberOfReceived = ev.ReceivedCount, NumberOfSent = ev.SentCount };
                sentEvents += rm.NumberOfSent;
                receivedEvents += rm.NumberOfReceived;
                remoteEventModel.Add(rm);
            }

            var activeServers = LicensingServices.Instance.GetService<IServerStateService>().GetActiveServers();

            var remoteServers = _serverStateService.CurrentServerState()
                .Where(x => x.Active)
                .ToArray();

            return new RemoteEventsModel()
            {
                RemoteEventModel = remoteEventModel,
                TotalNumberOfReceivedEvent = receivedEvents,
                TotalNumberOfSentEvent = sentEvents,
                ActiveServers = activeServers,
                ServerState = remoteServers,
                ProviderName = _providerService.DefaultProvider.Description ?? _providerService.DefaultProvider.Name,
                ProviderType = _providerService.DefaultProvider.GetType().FullName,
                Enabled = _providerService.Enabled
            };
        }
    }
}
