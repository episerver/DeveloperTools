using System.Collections.Generic;

namespace DeveloperTools.Models
{
    public class ModuleDependencyViewModel
    {
        public IEnumerable<ModuleInfo> Nodes { get; set; }

        public IEnumerable<ModuleDependency> Links { get; set; }

        public bool ShowAll { get; set; }
    }
}
