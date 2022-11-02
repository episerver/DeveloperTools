using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.DeveloperTools.Features.Common;
using EPiServer.DeveloperTools.Infrastructure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace EPiServer.DeveloperTools.Features.ViewLocations;

public class ViewLocationsController : DeveloperToolsController
{
    private readonly IOptions<RazorViewEngineOptions> _razorOptions;
    private readonly ICompositeViewEngine _viewEngine;

    public ViewLocationsController(IOptions<RazorViewEngineOptions> razorOptions, ICompositeViewEngine viewEngine)
    {
        _razorOptions = razorOptions;
        _viewEngine = viewEngine;
    }

    public ActionResult Index()
    {
        var options = _razorOptions.Value;
        var model = new ViewEngineLocationsModel();

        model.AreaViewLocationFormats.AddRange(options.AreaViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, "engineName")));
        model.AreaPageViewLocationFormats.AddRange(options.AreaPageViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, "engineName")));
        model.ViewLocations.AddRange(options.ViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, "engineName")));
        model.PageViewLocationFormats.AddRange(options.PageViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, "engineName")));

        foreach (var engine in _viewEngine.ViewEngines)
        {
            var cacheFieldInfo = engine.GetType().GetProperty("ViewLookupCache", BindingFlags.NonPublic | BindingFlags.Instance);
            if (cacheFieldInfo != null)
            {
                if (cacheFieldInfo.GetValue(engine) is IMemoryCache cache)
                {
                    var entriesFieldInfo = cache.GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (entriesFieldInfo != null)
                    {
                        if (entriesFieldInfo.GetValue(cache) is IDictionary entries)
                        {
                            foreach (var entry in entries.Keys)
                            {
                                model.ExpandedLocations.Add(
                                    new ExpandedLocationItemModel(
                                        entry.GetProperty("AreaName")?.ToString(),
                                        entry.GetProperty("ControllerName")?.ToString(),
                                        entry.GetProperty("PageName")?.ToString(),
                                        entry.GetProperty("ViewName")?.ToString()));
                            }
                        }
                    }
                }
            }
        }

        return View(model);
    }
}
