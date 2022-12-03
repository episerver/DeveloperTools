using System;

namespace EPiServer.DeveloperTools.Features.ModuleDependencies;

public class ModuleInfo
{
    public ModuleInfo()
    {
        Size = 10;
    }

    public string Id { get; set; }

    public int Group { get; set; }

    public int Size { get; set; }

    public Type ModuleType { get; set; }

    public string Label { get; set; }

    public string Title { get; set; }
}
