using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
