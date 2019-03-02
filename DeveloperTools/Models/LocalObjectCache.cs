using System.Collections.Generic;
using System.Web.Mvc;

namespace DeveloperTools.Models
{
    public class LocalObjectCache
    {
        public IEnumerable<LocalObjectCacheItem> CachedItems { get; set; }

        public string FilteredBy { get; set; }

        public IEnumerable<SelectListItem> Choices { get; set; }
    }

    public class LocalObjectCacheItem
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public long Size { get; set; }
    }
}
