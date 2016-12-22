using System.Collections.Generic;

namespace DeveloperTools.Models
{
    public class AssemblyInfo
    {
        public string Name { get; set; }
        public string AssemblyVersion { get; set; }
        public string FileVersion { get; set; }
        public string Location { get; set; }
    }

    public class AssembliesModel
    {
        public IEnumerable<AssemblyInfo> Assemblies { get; set; }
    }
}
