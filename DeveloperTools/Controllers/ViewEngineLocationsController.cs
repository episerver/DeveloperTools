using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DeveloperTools.Models;
using EPiServer.Shell.Web.Mvc;

namespace DeveloperTools.Controllers
{
    public class ViewEngineLocationsController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            var model = new ViewEngineLocationsModel();

            foreach (var engine in ViewEngines.Engines.OfType<VirtualPathProviderViewEngine>())
            {
                var engineName = engine.GetType().Name;
                model.ViewLocations.AddRange(engine.ViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
                model.PartialViewLocations.AddRange(engine.PartialViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
            }

            // special treatment for module view engine collection
            var modules = ViewEngines.Engines.OfType<ModuleViewEngineCollection>().FirstOrDefault();
            var memberInfo = modules?.GetType().GetField("_viewEngines", BindingFlags.NonPublic | BindingFlags.Instance);

            if(memberInfo != null)
            {
                var enginesCollection = memberInfo?.GetValue(modules) as Dictionary<string, IViewEngine>;
                if(enginesCollection != null)
                {
                    foreach (var engine in enginesCollection.Values.OfType<VirtualPathProviderViewEngine>())
                    {
                        var engineName = engine.GetType().Name;
                        model.ViewLocations.AddRange(engine.ViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
                        model.PartialViewLocations.AddRange(engine.PartialViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
                    }
                }
            }

            return View(model);
        }
    }
}
