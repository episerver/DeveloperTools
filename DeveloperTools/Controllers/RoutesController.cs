using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DeveloperTools.Models;
using EPiServer.Shell.Services.Rest;
using EPiServer.Shell.Web.Routing;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;

namespace DeveloperTools.Controllers
{
    public class RoutesController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            var routes = GetRouteModels();
            return View("Index", routes);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult FindRoute(string url)
        {
            var routes = GetRouteModels();
            foreach (var r in routes)
            {
                RouteData rd = null;
                try
                {
                    var ctx = new HttpContextWrapper(new HttpContext(new HttpRequest("", url, ""), new HttpResponse(new StringWriter(new StringBuilder()))));

                    if(r.Route is IContentRoute)
                    {
                        rd = ((Route) r.Route).GetRouteData(ctx);
                    }
                    else if(r.Route is ModuleRouteCollection)
                    {
                        rd = ((ModuleRouteCollection) r.Route).GetRouteData(ctx);
                    }
                    else if(r.Route is RestRoute)
                    {
                        rd = ((RestRoute) r.Route).GetRouteData(ctx);
                    }
                    else if(r.Route is Route)
                    {
                        rd = ((Route) r.Route).GetRouteData(ctx);
                    }
                    else
                    {
                        rd = r.Route.GetRouteData(ctx);
                    }
                }
                catch { }

                if(rd != null)
                {
                    r.DataTokens = GetValues("DataToken:", rd.DataTokens);
                    r.Values = GetValues("Values:", rd.Values);
                    r.IsSelected = true;
                }
            }
            return View("Index", routes);
        }

        private string GetValues(string title, RouteValueDictionary routeValueDictionary)
        {
            var s = new StringBuilder();
            s.Append(title);
            foreach (var rv in routeValueDictionary)
            {
                s.Append($"{rv.Key}:{rv.Value?.ToString() ?? "N/A"}  ,");
            }
            return s.ToString();
        }

        private IEnumerable<RouteModel> GetRouteModels()
        {
            var model = new List<RouteModel>();
            var index = 1;
            var namedRoutes = GetRouteNamedMap(RouteTable.Routes);
            foreach (var route in RouteTable.Routes)
            {
                var foundInNamed = namedRoutes.FirstOrDefault(r => r.Value == route);
                if(foundInNamed.Key != null)
                {
                    CreateRouteData(model, ref index, foundInNamed.Key, foundInNamed.Value);
                }
                else if(route != null)
                {
                    CreateRouteData(model, ref index, "No Name", route);
                }
            }

            return model;
        }

        private void CreateRouteData(List<RouteModel> model, ref int index, string routeName, RouteBase route)
        {
            var collection = route as ModuleRouteCollection;
            if(collection != null)
            {
                var m = CreateRouteModelData(routeName, collection, ref index);
                model.AddRange(m);
                return;
            }

            RouteModel rm;
            var rr = route as IContentRoute;
            if(rr != null)
            {
                var cr = rr;
                routeName = cr.Name;
                CreateRouteModelData(routeName, route as Route, index++);
            }

            if(route is RestRoute)
            {
                rm = CreateRouteModelData(routeName, route as RestRoute, index++);
            }
            else if(route is Route)
            {
                rm = CreateRouteModelData(routeName, route as Route, index++);
            }
            else
            {
                rm = CreateRouteModelData(routeName, route, index++);
            }

            model.Add(rm);
        }

        IEnumerable<RouteModel> CreateRouteModelData(string name, ModuleRouteCollection rr, ref int index)
        {
            var models = new List<RouteModel>();
            var rm = new RouteModel
            {
                Order = index.ToString(),
                RouteExistingFiles = GetRouteExistingFiles(rr),
                Name = name,
                Url = rr.RoutePath,
                Type = rr.GetType().FullName,
                Route = rr
            };

            models.Add(rm);
            index++;
            foreach (var r in rr)
            {
                CreateRouteData(models, ref index, name, r);
            }
            return models;
        }

        RouteModel CreateModel(RouteBase rb, int index)
        {
            var rm = new RouteModel
            {
                Order = index.ToString(),
                Type = rb.GetType().FullName,
                Route = rb,
                RouteExistingFiles = GetRouteExistingFiles(rb)
            };

            return rm;
        }

        RouteModel CreateRouteModelData(string name, RouteBase rr, int index)
        {
            var rm = CreateModel(rr, index);
            rm.Name = name;
            return rm;
        }

        RouteModel CreateRouteModelData(string name, RestRoute rr, int index)
        {
            var rm = CreateRouteModelData(name, rr as RouteBase, index);
            rm.Url = rr.Url;
            return rm;
        }

        RouteModel CreateRouteModelData(string name, Route rr, int index)
        {
            var rm = CreateRouteModelData(name, rr as RouteBase, index);
            if(rr is IContentRoute)
            {
                rm.Url = GetUrl(rr as IContentRoute);
            }
            else
            {
                rm.Url = GetUrl(rr);
            }

            rm.RouteHandler = rr.RouteHandler?.GetType().ToString();
            rm.Defaults = GetRouteValueDictionary(rr.Defaults);
            rm.Constraints = GetRouteValueDictionary(rr.Constraints);
            rm.DataTokens = GetRouteValueDictionary(rr.DataTokens);
            return rm;
        }

        private string GetRouteValueDictionary(RouteValueDictionary routeValueDictionary)
        {
            var sb = new StringBuilder();
            if(routeValueDictionary != null)
            {
                foreach (var kv in routeValueDictionary)
                {
                    sb.Append($"{kv.Key}:{kv.Value}");
                }
            }
            return sb.ToString();
        }

        private string GetRouteExistingFiles(RouteBase routeBase)
        {
            try
            {
                return
                    routeBase.GetType().InvokeMember("_routeExistingFiles", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, routeBase, null) as string;
            }
            catch { }

            return string.Empty;
        }

        private IDictionary<string, RouteBase> GetRouteNamedMap(RouteCollection routeCollection)
        {
            return
                routeCollection.GetType().InvokeMember("_namedMap", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, routeCollection, null) as
                    IDictionary<string, RouteBase>;
        }

        private static string GetUrl(IContentRoute cr)
        {
            try
            {
                var segemnts = cr.GetType().InvokeMember("_urlSegments", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, cr, null) as ISegment[];
                return string.Join("/", segemnts.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Name));
            }
            catch
            {
                return "No Url";
            }
        }

        private static string GetUrl(Route cr)
        {
            try
            {
                return cr.Url;
            }
            catch
            {
                return "No Url";
            }
        }
    }
}
