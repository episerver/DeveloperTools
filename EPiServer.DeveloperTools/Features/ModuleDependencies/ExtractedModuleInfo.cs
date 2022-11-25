using System;
using System.Collections.Generic;

namespace EPiServer.DeveloperTools.Features.ModuleDependencies
{
    internal class ExtractedModuleInfo
    {
        public Type Type { get; set; }

        public List<Type> Dependencies { get; set; }
    }
}
