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
        private static Dictionary<Guid, String> _mappedGuidName;
 
        public RemoteEventController(IEventRegistry eventRegistry, ServerStateService serverStateService, EventProviderService providerService)
        {
            _eventRegistry = eventRegistry;
            _serverStateService = serverStateService;
            _providerService = providerService;
            TranslateGuidToName();
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
                var rm = new RemoteEventModel() { Guid = ev.Id.ToString(), Name = GetName(ev.Id), NumberOfReceived = ev.ReceivedCount, NumberOfSent = ev.SentCount };
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

        private string GetName(Guid guid)
        {
            String name;
            if (_mappedGuidName.TryGetValue(guid, out name))
            {
                return name;
            }
            return guid.ToString();
        }

        private void TranslateGuidToName()
        {
            if (_mappedGuidName == null)
            {
                _mappedGuidName = new Dictionary<Guid, string>();
                _mappedGuidName.Add(new Guid("4e755664-8fd9-4906-88ca-476842076f98"), "CacheObjectStore-ObjectStoreCache");
                _mappedGuidName.Add(new Guid("1ee4c0b5-ca95-4bdb-b0d4-e5d9e91189aa"), "CacheEventNotifier-CacheEventNotifier");
                _mappedGuidName.Add(new Guid("c1b94788-2410-4da1-af51-4927abe5da94"), "PermanentLinkMapStore-Change");
                _mappedGuidName.Add(new Guid("414cda81-8720-41f1-bad2-7d6155f419dc"), "PermanentLinkMapStore-Remove");
                _mappedGuidName.Add(new Guid("ff174e9c-e3c4-4072-9e1f-7eaf59c5f54f"), "VirtualRoleReplication-Register");
                _mappedGuidName.Add(new Guid("546bf805-e87a-46d7-95d1-423ed87662bd"), "VirtualRoleReplication-UnRegister");
                _mappedGuidName.Add(new Guid("15fc0951-4510-49ae-9fa3-6cd4762d4101"), "VirtualRoleReplication-Clear");
                _mappedGuidName.Add(new Guid("f67ab721-faa5-4e37-b894-75aaffa7665d"), "VisitorGroupEvents-Saved");
                _mappedGuidName.Add(new Guid("719a93e5-727e-441f-8167-70cca8639803"), "VisitorGroupEvents-Deleted");
                _mappedGuidName.Add(new Guid("eaac6db8-c224-4558-8e07-4454a15f8d71"), "RuntimeCacheEvents-BlockedCache");
                _mappedGuidName.Add(new Guid("69793f9f-c106-4b71-a524-8ed7af051077"), "RuntimeCacheEvents-FlushStoredCache");
                _mappedGuidName.Add(new Guid("d464d910-68ef-402a-98c6-72a2c95dcdba"), "ContentLanguageSettingsHandler-ClearSettings");
                _mappedGuidName.Add(new Guid("0ccfcc21-2e73-4901-a559-a4484d7bd472"), "ContentLanguageSettingsHandler-ClearTreeMap");
                _mappedGuidName.Add(new Guid("c6fb4f08-069c-4ee6-8588-4d5cc806b654"), "BroadcastOperations-Workflow");
                _mappedGuidName.Add(new Guid("b6f0e39a-93ef-4dee-a728-a49dac5501fa"), "ChangeLogSystem-Start");
                _mappedGuidName.Add(new Guid("a416a5af-6469-48f7-ad9e-c358fe506916"), "ChangeLogSystem-Stop");
                _mappedGuidName.Add(new Guid("55a261f2-9b2a-47dc-87b7-29c4eab895eb"), "ChangeLogSystem-StateChange");
                _mappedGuidName.Add(new Guid("9484e34b-b419-4e59-8fd5-3277668a7fce"), "RemoteCacheSynchronization-RemoveFromCache");
                _mappedGuidName.Add(new Guid("51da5053-6af8-4a10-9bd4-8417e48f38bd"), "ServerStateService-State");
                _mappedGuidName.Add(new Guid("184468e9-9f0d-4460-aecd-3c08f652c73c"), "ScheduledJob-StopJob");

                _mappedGuidName.Add(new Guid("CCDF4F50-8216-4919-B8C1-21D9F1932BAF"), "MetaDataEventManager-MetaDataUpdated");
                _mappedGuidName.Add(new Guid("8B448BC6-F7E1-4833-BDC7-CA338B77ADFA"), "ProductEventManager-CommerceProductUpdated");
                _mappedGuidName.Add(new Guid("B10915E6-0C84-4a6a-8707-FF6F357A1099"), "BlogModule-BlogReplication");
                _mappedGuidName.Add(new Guid("F6F0147E-F60F-4801-8647-66270D10AFB9"), "ForumModule-ForumReplication");
                _mappedGuidName.Add(new Guid("F6742777-6F38-46a1-AA38-9985715089A2"), "ImageGalleryModule-ImageGalleryReplication");
                _mappedGuidName.Add(new Guid("F160271E-7972-447a-81D0-152A35FD77BD"), "OnlineStatusModule-OnlineStatusReplication");
            }
        }
    }
}