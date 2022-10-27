using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeveloperTools.Models;
using EPiServer.DeveloperTools.Features.Common;
using EPiServer.Shell.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Options;

namespace DeveloperTools.Controllers
{
    public class ViewEngineLocationsController : DeveloperToolsController
    {
        private readonly IOptions<RazorViewEngineOptions> _razorOptions;

        public ViewEngineLocationsController(IOptions<RazorViewEngineOptions> razorOptions) { _razorOptions = razorOptions; }

        public ActionResult Index()
        {
            var model = new ViewEngineLocationsModel();

            foreach (var engine in _razorOptions.Value.ViewLocationExpanders)
            {
                var engineName = engine.GetType().Name;
                //model.ViewLocations.AddRange(engine.ViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
                //model.PartialViewLocations.AddRange(engine.PartialViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
            }

            // special treatment for module view engine collection
            //var modules = ViewEngines.Engines.OfType<ModuleViewEngineCollection>().FirstOrDefault();
            //var memberInfo = modules?.GetType().GetField("_viewEngines", BindingFlags.NonPublic | BindingFlags.Instance);

            //if(memberInfo != null)
            //{
            //    var enginesCollection = memberInfo?.GetValue(modules) as Dictionary<string, IViewEngine>;
            //    if(enginesCollection != null)
            //    {
            //        foreach (var engine in enginesCollection.Values.OfType<VirtualPathProviderViewEngine>())
            //        {
            //            var engineName = engine.GetType().Name;
            //            model.ViewLocations.AddRange(engine.ViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
            //            model.PartialViewLocations.AddRange(engine.PartialViewLocationFormats.Select(f => new ViewEngineLocationItemModel(f, engineName)));
            //        }
            //    }
            //}

            return View(model);
        }
    }
}
