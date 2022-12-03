using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPiServer.DeveloperTools.Features.LocalObjectCache;

public class LocalObjectCacheModel
{
    public IEnumerable<LocalObjectCacheModelItem> CachedItems { get; set; } = new List<LocalObjectCacheModelItem>();

    public string FilteredBy { get; set; }

    public IEnumerable<SelectListItem> Choices { get; set; } = new List<SelectListItem>();

    public bool ViewObjectSize { get; set; }
}

public class LocalObjectCacheModelItem
{
    public string Key { get; set; }

    public object Value { get; set; }

    public long Size { get; set; }
}
