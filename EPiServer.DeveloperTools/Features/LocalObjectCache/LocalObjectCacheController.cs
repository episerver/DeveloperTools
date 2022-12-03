using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DeveloperTools.Features.Common;
using EPiServer.DeveloperTools.Infrastructure;
using EPiServer.Framework.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace EPiServer.DeveloperTools.Features.LocalObjectCache;

public class LocalObjectCacheController : DeveloperToolsController
{
    private readonly ISynchronizedObjectInstanceCache _cache;
    private readonly IMemoryCache _memoryCache;

    public LocalObjectCacheController(ISynchronizedObjectInstanceCache cache, IMemoryCache memoryCache)
    {
        _cache = cache;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    [HttpPost]
    public ActionResult Index(string filteredBy, bool os = false)
    {
        return View(PrepareViewModel(filteredBy, os));
    }

    [HttpPost]
    public ActionResult RemoveLocalCache(string[] cacheKeys, bool os)
    {
        if (cacheKeys != null)
        {
            foreach (var key in cacheKeys)
            {
                _cache.RemoveLocal(key);
            }
        }

        return RedirectToAction(nameof(Index), new RouteValueDictionary(new { os }));
    }

    [HttpPost]
    public ActionResult RemoveLocalRemoteCache(string[] cacheKeys, bool os)
    {
        if (cacheKeys != null)
        {
            foreach (var key in cacheKeys)
            {
                _cache.RemoveLocal(key);
                _cache.RemoveRemote(key);
            }
        }

        return RedirectToAction(nameof(Index), new RouteValueDictionary(new { os }));
    }

    [HttpPost]
    public ActionResult ViewObjectSize()
    {
        return RedirectToAction(nameof(Index), new RouteValueDictionary(new { os = true }));
    }

    private LocalObjectCacheModel PrepareViewModel(string filteredBy, bool viewObjectSize)
    {
        var model = new LocalObjectCacheModel();

        // try to get in-memory cache object
        if (_memoryCache is MemoryCache memCache)
        {
            var entriesFieldInfo = memCache.GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
            if (entriesFieldInfo != null && entriesFieldInfo.GetValue(memCache) is IDictionary entries)
            {
                model.CachedItems = ConvertToListItem(entries, viewObjectSize, filteredBy);
            }
        }

        model.FilteredBy = filteredBy;
        model.Choices = new[]
        {
            new SelectListItem { Text = "All Cached Objects", Value = "all" },
            new SelectListItem { Text = "Any Content", Value = "content" },
            new SelectListItem { Text = "Pages Only", Value = "pages" },
            new SelectListItem { Text = "Property Definitions Only", Value = "properties" },
        };

        model.ViewObjectSize = viewObjectSize;

        return model;
    }

    private IEnumerable<LocalObjectCacheModelItem> ConvertToListItem(
        IDictionary cachedEntries,
        bool viewObjectSize,
        string filteredBy)
    {
        var result = new List<LocalObjectCacheModelItem>();

        foreach (var key in cachedEntries.Keys)
        {
            var value = cachedEntries[key].GetProperty("Value");

            switch (filteredBy)
            {
                case "content":
                    value = value as IContent;
                    break;
                case "pages":
                    value = value as PageData;
                    break;
                case "properties":
                    value = value as PropertyDefinition;
                    break;
            }

            if (value != null)
            {
                result.Add(
                    new LocalObjectCacheModelItem
                    {
                        Key = key.ToString(), Value = value, Size = viewObjectSize ? GetObjectSize(value) : 0
                    });
            }
        }

        return result;
    }

    private static long GetObjectSize(object obj)
    {
        if (obj == null)
        {
            return 0;
        }

        try
        {
            using Stream s = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(s, obj);

            return s.Length;
        }
        catch (Exception)
        {
            return -1;
        }
    }
}
