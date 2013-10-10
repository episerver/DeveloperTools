using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace DeveloperTools.Models
{
    [Serializable]
    public class RemoteEventsModel
    {
        public RemoteEventsModel()
        {
            RemoteEventModel =    Enumerable.Empty<RemoteEventModel>();
            SendRemoteEventModel = new SendRemoteEventModel();
        }

        public IEnumerable<RemoteEventModel> RemoteEventModel {get;set;}
        public long TotalNumberOfSentEvent { get; set; }
        public long TotalNumberOfReceivedEvent { get; set; }
        public SendRemoteEventModel SendRemoteEventModel { get; set; }
    }

    [Serializable]
    public class RemoteEventModel
    {
        public String Name { get; set; }
        public long NumberOfSent { get; set; }
        public long NumberOfReceived { get; set; }
    }

    [Serializable]
    public class SendRemoteEventModel
    {
        public Guid EventId { get; set; }
        public string Param { get; set; }
        public int NumberOfevents { get; set; }
        public int NumberOfeventsSent { get; set; }

    }
}
