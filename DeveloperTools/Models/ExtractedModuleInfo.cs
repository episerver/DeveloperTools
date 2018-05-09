using System;
using System.Collections.Generic;

namespace DeveloperTools.Models
{
    internal class ExtractedModuleInfo
    {
        public Type Type { get; set; }

        public List<Type> Dependencies { get; set; }
    }
}
