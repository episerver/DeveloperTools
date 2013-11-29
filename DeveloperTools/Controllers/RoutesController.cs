using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Shell.Gadgets;
using System.Web.Mvc;
using EPiServer.Framework.Initialization;
using EPiServer.Shell.Web;
using System.Web.Routing;
using DeveloperTools.Models;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;
using EPiServer.Shell.Services.Rest;
using EPiServer.Shell.Web.Routing;
using System.Web;
using System.IO;

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
        public ActionResult FindRoute(String url)
        {
            var routes = GetRouteModels();
            foreach(var r in routes)
            {
                RouteData rd = null;
                try
                {
                    var ctx = new HttpContextWrapper(new HttpContext (new HttpRequest("", url, ""), new HttpResponse(new StringWriter(new StringBuilder()))));
                    
                    if (r.Route is ContentRoute)
                    {
                        rd = (r.Route as ContentRoute).GetRouteData(ctx);
                    }
                    else if (r.Route is ModuleRouteCollection)
                    {
                        rd = (r.Route as ModuleRouteCollection).GetRouteData(ctx);
                    }
                    else if (r.Route is RestRoute)
                    {
                        rd = (r.Route as RestRoute).GetRouteData(ctx);
                    }
                    else if (r.Route is Route)
                    {
                        rd = (r.Route as Route).GetRouteData(ctx);
                    }
                    else
                    {
                        rd = (r.Route as RouteBase).GetRouteData(ctx);
                    }
                }
                catch { }
                if (rd !=  null)
                {
                    r.DataTokens = GetValues("DataToken:",rd.DataTokens);
                    r.Values = GetValues("Values:", rd.Values);
                    r.IsSelected = true;
                }
            }
            return View("Index", routes);
        }

        private string GetValues(String title, RouteValueDictionary routeValueDictionary)
        {
            StringBuilder s = new StringBuilder();
            s.Append(title);
            int index = 0;
            foreach (var rv in routeValueDictionary)
            {
                index++;
                s.Append(String.Format("{0}:{1}  ,",  rv.Key, rv.Value != null ? rv.Value.ToString() : "N/A"));
            }
            return s.ToString();
        }

        private IEnumerable<RouteModel> GetRouteModels()
        {
            List<RouteModel> model = new List<RouteModel>();
            int index = 1;
            var namedRoutes = GetRouteNamedMap(RouteTable.Routes);
            foreach (var route in RouteTable.Routes)
            {
                var foundInNamed = namedRoutes.Where(r => r.Value == route).FirstOrDefault();
                if (foundInNamed.Key != null)
                {
                    CreateRouteData(model, ref index, foundInNamed.Key, foundInNamed.Value);
                }
                else if (route != null)
                {
                    CreateRouteData(model, ref index, "No Name", route);
                }
            }
            return model;
        }

        private void CreateRouteData(List<RouteModel> model, ref int index, String routeName, RouteBase route)
        {
            if (route is ModuleRouteCollection)
            {
                var m = CreateRouteModelData(routeName, route as ModuleRouteCollection, ref index);
                model.AddRange(m);
                return;
            }
            RouteModel rm;
            if (route is ContentRoute)
            {
                var cr = route as ContentRoute;
                routeName = cr.Name;
                CreateRouteModelData(routeName, route as Route, index++);
            }
            
            if (route is RestRoute)
            {
               rm =  CreateRouteModelData(routeName, route as RestRoute, index++);
            }
            else if (route is Route)
            {
                rm = CreateRouteModelData(routeName, route as Route, index++);
            }
            else 
            {
                rm = CreateRouteModelData(routeName, route as RouteBase, index++);
            }
            
            model.Add(rm);
        }

        IEnumerable<RouteModel> CreateRouteModelData(String name, ModuleRouteCollection rr, ref int index)
        {
            List<RouteModel> models = new List<RouteModel>();
            RouteModel rm = new RouteModel();
            rm.Order = index.ToString();
            rm.RouteExistingFiles = GetRouteExistingFiles(rr);
            rm.Name = name;
            rm.Url = rr.RoutePath;
            rm.Type = rr.GetType().FullName;
            rm.Route = rr;
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
            RouteModel rm = new RouteModel();
            rm.Order = index.ToString();
            rm.Type = rb.GetType().FullName;
            rm.Route = rb;
            rm.RouteExistingFiles = GetRouteExistingFiles(rb);
            return rm;
        }

        RouteModel CreateRouteModelData(String name, RouteBase rr, int index)
        {
            RouteModel rm = CreateModel(rr, index);
            rm.Name = name;
            return rm;
        }

        RouteModel CreateRouteModelData(String name, RestRoute rr, int index)
        {
            RouteModel rm = CreateRouteModelData(name, rr as RouteBase, index);
            rm.Url = rr.Url;
            return rm;
        }

        RouteModel CreateRouteModelData(String name, Route rr, int index)
        {
            RouteModel rm = CreateRouteModelData(name, rr as RouteBase, index);
            if (rr is ContentRoute)
            {
                rm.Url = GetUrl(rr as ContentRoute);
            }
            else
            {
                rm.Url = GetUrl(rr);
            }

            rm.RouteHandler = rr.RouteHandler.GetType().ToString();
            rm.Defaults = GetRouteValueDictionary(rr.Defaults);
            rm.Constraints = GetRouteValueDictionary(rr.Constraints);
            rm.DataTokens = GetRouteValueDictionary(rr.DataTokens);
            return rm;
        }

        private string GetRouteValueDictionary(RouteValueDictionary routeValueDictionary)
        {

            StringBuilder sb = new StringBuilder();
            if (routeValueDictionary != null)
            {
                foreach (var kv in routeValueDictionary)
                {
                    sb.Append(String.Format("{0}:{1}", kv.Key, kv.Value));
                }
            }
            return sb.ToString();
        }

        private string GetRouteExistingFiles(RouteBase routeBase)
        {
            try
            {
                return routeBase.GetType().InvokeMember("_routeExistingFiles", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, routeBase, null) as string;
            }
            catch { }
            return String.Empty;
        }

        private IDictionary<string, RouteBase> GetRouteNamedMap(RouteCollection routeCollection)
        {
            return routeCollection.GetType().InvokeMember("_namedMap", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, routeCollection, null) as IDictionary<string, RouteBase>;
        }

        private static string GetUrl(ContentRoute cr)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                ISegment[] segemnts = cr.GetType().InvokeMember("_urlSegments", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, cr, null) as ISegment[];
                return String.Join("/", segemnts.Where(s => !String.IsNullOrEmpty(s.Name)).Select(s => s.Name));
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
