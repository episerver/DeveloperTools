using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EPiServer.DeveloperTools.Features.Common;
using EPiServer.DeveloperTools.Infrastructure;
using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Newtonsoft.Json;

namespace EPiServer.DeveloperTools.Features.Routes;

public class RoutesController : DeveloperToolsController
{
    private readonly IEnumerable<EndpointDataSource> _endpointSources;

    public RoutesController(IEnumerable<EndpointDataSource> endpointSources)
    {
        _endpointSources = endpointSources;
    }

    public ActionResult Index()
    {
        var routes = GetRouteModels();
        return View("Index", routes);
    }

    //[HttpPost, ActionName("Index")]
    //public ActionResult FindRoute(string url)
    //{
    //    var routes = GetRouteModels();
    //    //foreach (var r in routes)
    //    //{
    //    //    RouteData rd = null;
    //    //    try
    //    //    {
    //    //        var ctx = new HttpContextWrapper(new HttpContext(new HttpRequest("", url, ""), new HttpResponse(new StringWriter(new StringBuilder()))));

    //    //        if(r.Route is IContentRoute)
    //    //        {
    //    //            rd = ((Route) r.Route).GetRouteData(ctx);
    //    //        }
    //    //        else if(r.Route is ModuleRouteCollection)
    //    //        {
    //    //            rd = ((ModuleRouteCollection) r.Route).GetRouteData(ctx);
    //    //        }
    //    //        else if(r.Route is RestRoute)
    //    //        {
    //    //            rd = ((RestRoute) r.Route).GetRouteData(ctx);
    //    //        }
    //    //        else if(r.Route is Route)
    //    //        {
    //    //            rd = ((Route) r.Route).GetRouteData(ctx);
    //    //        }
    //    //        else
    //    //        {
    //    //            rd = r.Route.GetRouteData(ctx);
    //    //        }
    //    //    }
    //    //    catch { }

    //    //    if(rd != null)
    //    //    {
    //    //        r.Parameters = GetValues("Parameters:", rd.Parameters);
    //    //        r.Values = GetValues("Values:", rd.Values);
    //    //        r.IsSelected = true;
    //    //    }
    //    //}
    //    return View("Index", routes);
    //}

    //private string GetValues(string title, RouteValueDictionary routeValueDictionary)
    //{
    //    return $"{title}{string.Join(", ", routeValueDictionary.Select(kv => $"{kv.Key}:{kv.Value?.ToString() ?? "N/A"}"))}";
    //}

    private IEnumerable<RouteModel> GetRouteModels()
    {
        var model = new List<RouteModel>();

        var endpointGroups = _endpointSources
            .SelectMany(_ => _.Endpoints)
            .Cast<RouteEndpoint>()
            .GroupBy(_ => _.DisplayName);

        foreach (var endpointGroup in endpointGroups)
        {
            var endpoint = endpointGroup.First();
            model.Add(new RouteModel
            {
                Name = endpoint.DisplayName,
                RouteHandler = endpoint.RoutePattern.RequiredValues.FirstOrDefault(rv => rv.Key.Equals("controller", StringComparison.OrdinalIgnoreCase)).Value?.ToString() ?? string.Empty,
                Url = endpoint.RoutePattern.RawText,
                Defaults = GetEndpointDefault(endpoint.RoutePattern),
                Parameters = GetEndpointParameters(endpoint.RoutePattern),
                Methods = GetEndpointMethods(endpointGroup),
                Order = endpoint.Order.ToString(),
            });
        }

        return model;
    }

    private string GetEndpointParameters(RoutePattern pattern)
    {
        return string.Join(", ",
                           pattern.Parameters.Select(_ => _.Name));
    }

    private string GetEndpointMethods(IGrouping<string, Endpoint> endpointGroup) =>
        string.Join(",",
                    endpointGroup
                        .Select(_ => _.Metadata.GetMetadata<HttpMethodMetadata>())
                        .Select(_ => _ == null ? new[] { "GET" } : _.HttpMethods)
                        .SelectMany(_ => _)
                        .Distinct());

    private string GetEndpointDefault(RoutePattern pattern)
    {
        var defaults = new List<string>();

        foreach (var patternDefault in pattern.Defaults)
        {
            if (patternDefault.Key.Equals("module", StringComparison.InvariantCultureIgnoreCase))
            {
                if (patternDefault.Value is ShellModule moduleInfo)
                {
                    defaults.Add($"{patternDefault.Key}={moduleInfo.Name} (AuthorizationPolicy={moduleInfo.AuthorizationPolicy}, ClientAuthorizationPolicy={moduleInfo.ClientAuthorizationPolicy})");
                }
            }
            else
            {
                defaults.Add($"{patternDefault.Key}={patternDefault.Value}");
            }
        }

        return $"[{string.Join(", ", defaults)}]";
    }
}
