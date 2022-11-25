using System.Collections.Generic;

namespace EPiServer.DeveloperTools.Features.IoC;

public class IOCModel
{
    public IEnumerable<IOCEntry> IOCEntries { get; set; } = new List<IOCEntry>();
}

public class IOCEntry
{
    public bool IsDefault { get; set; }
    public string PluginType { get; set; }
    public string ConcreteType { get; set; }
    public string ImplementationFactory { get; set; }
    public string Scope { get; set; }
    public string ImplementationInstance { get; set; }
}
