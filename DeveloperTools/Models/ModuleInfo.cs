using System;

namespace DeveloperTools.Models
{
    public class ModuleInfo
    {
        public ModuleInfo()
        {
            Value = 5;
        }

        public string Id { get; set; }

        public int Group { get; set; }

        public int Value { get; set; }

        public Type ModuleType { get; set; }

        public string Label { get; set; }

        public string Title { get; set; }
    }
}
