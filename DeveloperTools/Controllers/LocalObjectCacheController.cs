using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using System.Web.Routing;
using DeveloperTools.Core;
using DeveloperTools.Models;
using EPiServer.Core;
using EPiServer.Framework.Cache;

namespace DeveloperTools.Controllers
{
    public class LocalObjectCacheController : DeveloperToolsController
    {
        private readonly ISynchronizedObjectInstanceCache _cache;

        public LocalObjectCacheController(ISynchronizedObjectInstanceCache cache)
        {
            _cache = cache;
        }

        public ActionResult Index(string FilteredBy, bool os = false)
        {
            return View(PrepareViewModel(FilteredBy, os));
        }

        [HttpParamAction]
        public ActionResult RemoveLocalCache(string[] cacheKeys, bool os)
        {
            if(cacheKeys != null)
            {
                foreach (string key in cacheKeys)
                {
                    _cache.RemoveLocal(key);
                }
            }

            return RedirectToAction("Index", new RouteValueDictionary(new { os }));
        }

        [HttpParamAction]
        public ActionResult RemoveLocalRemoteCache(string[] cacheKeys, bool os)
        {
            if(cacheKeys != null)
            {
                foreach (string key in cacheKeys)
                {
                    _cache.RemoveLocal(key);
                    _cache.RemoveRemote(key);
                }
            }

            return RedirectToAction("Index", new RouteValueDictionary(new { os }));
        }

        [HttpParamAction]
        public ActionResult ViewObjectSize()
        {
            return RedirectToAction("Index", new RouteValueDictionary(new { os = true }));
        }

        private LocalObjectCache PrepareViewModel(string FilteredBy, bool viewObjectSize)
        {
            var model = new LocalObjectCache();

            var cachedEntries = HttpContext.Cache.Cast<DictionaryEntry>();

            switch (FilteredBy)
            {
                case "pages":
                    model.CachedItems = ConvertToListItem(cachedEntries.Where(item => item.Value is PageData), viewObjectSize);
                    break;
                case "content":
                    model.CachedItems = ConvertToListItem(cachedEntries.Where(item => item.Value is IContent), viewObjectSize);
                    break;
                default:
                    model.CachedItems = ConvertToListItem(cachedEntries, viewObjectSize);
                    break;
            }

            model.FilteredBy = FilteredBy;
            model.Choices = new[]
                            {
                                new SelectListItem { Text = "All Cached Objects", Value = "all" },
                                new SelectListItem { Text = "Any Content", Value = "content" },
                                new SelectListItem { Text = "Pages Only", Value = "pages" }
                            };

            model.ViewObjectSize = viewObjectSize;
            return model;
        }

        private IEnumerable<LocalObjectCacheItem> ConvertToListItem(IEnumerable<DictionaryEntry> cachedEntries, bool viewObjectSize) =>
            cachedEntries.Select(e => new LocalObjectCacheItem
                                      {
                                          Key = e.Key.ToString(),
                                          Value = e.Value,
                                          Size = viewObjectSize ? GetObjectSize(e.Value) : 0
                                      }).ToList();

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
