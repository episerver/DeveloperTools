using System.Collections.Generic;

namespace DeveloperTools.Models
{
    public class ModuleDependencyViewModel
    {
        public ICollection<ModuleInfo> Nodes { get; set; }

        public ICollection<ModuleDependency> Links { get; set; }

        public bool ShowAll { get; set; }
    }
}
