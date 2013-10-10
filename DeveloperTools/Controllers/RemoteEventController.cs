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

namespace DeveloperTools.Controllers
{
    public class RemoteEventController : DeveloperToolsController
    {
        static Guid RaiserId = Guid.NewGuid();
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
                Event e = new Event(sendRemoteEventModel.EventId);
                for (; i < sendRemoteEventModel.NumberOfevents; i++)
                {
                    e.Raise(RaiserId, sendRemoteEventModel.Param);
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
            foreach (var ev in Event.GetAll())
            {
                var rm = new RemoteEventModel() { Name = ev.Id.ToString(), NumberOfReceived = RemoteEventsManager.GetReceivedCount(ev.Id), NumberOfSent = RemoteEventsManager.GetSentCount(ev.Id) };
                sentEvents += rm.NumberOfSent;
                receivedEvents += rm.NumberOfReceived;
                remoteEventModel.Add(rm);
            }
            var model = new RemoteEventsModel() { RemoteEventModel = remoteEventModel, TotalNumberOfReceivedEvent = receivedEvents, TotalNumberOfSentEvent = sentEvents };
            return model;
        }
    }
}
