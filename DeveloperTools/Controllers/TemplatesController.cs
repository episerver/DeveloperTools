using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeveloperTools.Controllers
{
    public class TemplatesController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            var templateRepo = ServiceLocator.Current.GetInstance<TemplateModelRepository>();
            var modelRepo = ServiceLocator.Current.GetInstance<ContentTypeModelRepository>();

            var templates = modelRepo.List()
                .Where(ct => ct.ModelType != null)
                .SelectMany(ct => templateRepo.List(ct.ModelType))
                .GroupBy(t => GetHashCode(t.Name, t.Path, t.TemplateType, t.TemplateTypeCategory))
                .Select(g => g.First());
                

            return View(templates);
        }

        private static int GetHashCode(params object[] objects)
        {
            int first = objects.First().GetHashCode();
            foreach (var o in objects.Skip(1))
            {
                if (o == null)
                {
                    continue;
                }
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + first;
                    hash = hash * 31 + o.GetHashCode();
                    first = hash;
                }
            }
            return first;
        }

    }
}
