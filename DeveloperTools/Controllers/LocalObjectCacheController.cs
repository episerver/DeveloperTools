using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using DeveloperTools.Core;
using DeveloperTools.Models;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.PlugIn;

namespace DeveloperTools.Controllers
{
    [Authorize(Roles = "CmsAdmins")]
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Url = "~/localobjectcache", DisplayName = "Clear Local Object Cache")]
    public class LocalObjectCacheController : DeveloperToolsController
    {
        private readonly ISynchronizedObjectInstanceCache _cache;

        public LocalObjectCacheController(ISynchronizedObjectInstanceCache cache)
        {
            _cache = cache;
        }

        public ActionResult Index(string FilteredBy)
        {
            var model = new LocalObjectCache();

            var cachedEntries = HttpContext.Cache.Cast<DictionaryEntry>();

            switch (FilteredBy)
            {
                case "pages":
                    model.CachedItems = ConvertToListItem(cachedEntries.Where(item => item.Value is PageData));
                    break;
                case "content":
                    model.CachedItems = ConvertToListItem(cachedEntries.Where(item => item.Value is IContent));
                    break;
                default:
                    model.CachedItems = ConvertToListItem(cachedEntries);
                    break;
            }

            model.FilteredBy = FilteredBy;
            model.Choices = new[]
                            {
                                new SelectListItem { Text = "All Cached Objects", Value = "all" },
                                new SelectListItem { Text = "Any Content", Value = "content" },
                                new SelectListItem { Text = "Pages Only", Value = "pages" }
                            };

            return View(model);
        }

        private IEnumerable<LocalObjectCacheItem> ConvertToListItem(IEnumerable<DictionaryEntry> cachedEntries) =>
            cachedEntries.Select(e => new LocalObjectCacheItem
                                      {
                                          Key = e.Key.ToString(),
                                          Value = e.Value,
                                          Size = GetObjectSize(e.Value)
                                      }).ToList();

        [HttpParamAction]
        public ActionResult RemoveLocalCache(string[] cacheKeys)
        {
            if(cacheKeys != null)
            {
                foreach (string key in cacheKeys)
                {
                    _cache.RemoveLocal(key);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpParamAction]
        public ActionResult RemoveLocalRemoteCache(string[] cacheKeys)
        {
            if(cacheKeys != null)
            {
                foreach (string key in cacheKeys)
                {
                    _cache.RemoveLocal(key);
                    _cache.RemoveRemote(key);
                }
            }

            return RedirectToAction("Index");
        }

        private static long GetObjectSize(object obj)
        {
            if(obj == null)
                return 0;

            try
            {
                using (Stream s = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(s, obj);

                    return s.Length;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
