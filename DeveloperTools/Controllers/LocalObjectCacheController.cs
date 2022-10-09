using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DeveloperTools.Models;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace DeveloperTools.Controllers
{
    public class LocalObjectCacheController : DeveloperToolsController
    {
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly IMemoryCache _memoryCache;

        public LocalObjectCacheController(ISynchronizedObjectInstanceCache cache, IMemoryCache memoryCache)
        {
            _cache = cache;
            _memoryCache = memoryCache;
        }

        public ActionResult Index(string FilteredBy, bool os = false)
        {
            return View(PrepareViewModel(FilteredBy, os));
        }

        //[HttpParamAction]
        [HttpPost]
        public ActionResult RemoveLocalCache(string[] cacheKeys, bool os)
        {
            if(cacheKeys != null)
            {
                foreach (var key in cacheKeys)
                {
                    _cache.RemoveLocal(key);
                }
            }

            return RedirectToAction(nameof(Index), new RouteValueDictionary(new { os }));
        }

        //[HttpParamAction]
        [HttpPost]
        public ActionResult RemoveLocalRemoteCache(string[] cacheKeys, bool os)
        {
            if(cacheKeys != null)
            {
                foreach (var key in cacheKeys)
                {
                    _cache.RemoveLocal(key);
                    _cache.RemoveRemote(key);
                }
            }

            return RedirectToAction(nameof(Index), new RouteValueDictionary(new { os }));
        }

        //[HttpParamAction]
        [HttpPost]
        public ActionResult ViewObjectSize()
        {
            return RedirectToAction(nameof(Index), new RouteValueDictionary(new { os = true }));
        }

        private LocalObjectCache PrepareViewModel(string FilteredBy, bool viewObjectSize)
        {
            var model = new LocalObjectCache();

            //var cachedEntries = _memoryCache.Cast<DictionaryEntry>().Take(10_000);

            //switch (FilteredBy)
            //{
            //    case "pages":
            //        model.CachedItems = ConvertToListItem(cachedEntries.Where(item => item.Value is PageData), viewObjectSize);
            //        break;
            //    case "content":
            //        model.CachedItems = ConvertToListItem(cachedEntries.Where(item => item.Value is IContent), viewObjectSize);
            //        break;
            //    default:
            //        model.CachedItems = ConvertToListItem(cachedEntries, viewObjectSize);
            //        break;
            //}

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
