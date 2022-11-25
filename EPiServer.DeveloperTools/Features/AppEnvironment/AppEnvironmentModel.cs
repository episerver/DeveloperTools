using System.Collections.Generic;

namespace EPiServer.DeveloperTools.Features.AppEnvironment;

public class AppEnvironmentModel
{
    public IEnumerable<AssemblyInfo> Assemblies { get; set; }
    public IReadOnlyDictionary<string, string> Misc { get; set; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> ConnectionString { get; set; }
}

public class AssemblyInfo
{
    public string Name { get; set; }
    public string AssemblyVersion { get; set; }
    public string FileVersion { get; set; }
    public string Location { get; set; }
}
