using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperTools.Controllers
{
    public class TemplatesController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            //var templateRepo = ServiceLocator.Current.GetInstance<ITemplateRepository>();
            var modelRepo = ServiceLocator.Current.GetInstance<ContentTypeModelRepository>();

            var templates = modelRepo.List();
                                     //.Where(ct => ct.ModelType != null)
                                     //.SelectMany(ct => templateRepo.List(ct.ModelType))
                                     //.GroupBy(t => GetHashCode(t.Name, t.Path, t.TemplateType, t.TemplateTypeCategory))
                                     //.Select(g => g.First());

            return View(templates);
        }

        private static int GetHashCode(params object[] objects)
        {
            var first = objects.First().GetHashCode();
            foreach (var o in objects.Skip(1))
            {
                if(o == null)
                {
                    continue;
                }
                unchecked
                {
                    var hash = 17;
                    hash = hash * 31 + first;
                    hash = hash * 31 + o.GetHashCode();
                    first = hash;
                }
            }

            return first;
        }
    }
}
