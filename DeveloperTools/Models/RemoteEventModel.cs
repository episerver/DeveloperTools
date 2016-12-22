using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Events.Clients.Internal;

namespace DeveloperTools.Models
{
    [Serializable]
    public class RemoteEventsModel
    {
        public RemoteEventsModel()
        {
            RemoteEventModel = Enumerable.Empty<RemoteEventModel>();
            SendRemoteEventModel = new SendRemoteEventModel();
        }

        public IEnumerable<RemoteEventModel> RemoteEventModel { get; set; }
        public long TotalNumberOfSentEvent { get; set; }
        public long TotalNumberOfReceivedEvent { get; set; }
        public SendRemoteEventModel SendRemoteEventModel { get; set; }
        public IEnumerable<string> ActiveServers { get; set; }
        public IEnumerable<ServerState> ServerState { get; set; }
        public string ProviderName { get; set; }
        public string ProviderType { get; set; }
        public bool Enabled { get; set; }
    }

    [Serializable]
    public class RemoteEventModel
    {
        public string Name { get; set; }
        public string Guid { get; set; }
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
