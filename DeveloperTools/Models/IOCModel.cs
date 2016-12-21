using System.Collections.Generic;

namespace DeveloperTools.Models
{
    public class IOCEntry
    {
        public bool IsDefault { get; set; }
        public string PluginType { get; set; }
        public string ConcreteType { get; set; }
        public string Scope { get; set; }
    }

    public class IOCModel
    {
        public IEnumerable<IOCEntry> IOCEntries { get; set; }
        public IEnumerable<string> LoadingErrors { get; set; }
    }
}
