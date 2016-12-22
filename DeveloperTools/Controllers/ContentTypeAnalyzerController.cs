using System.Web.Mvc;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace DeveloperTools.Controllers
{
    public class ContentTypeAnalyzerController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            return View(ServiceLocator.Current.GetInstance<ContentTypeModelRepository>().List());
        }
    }
}
