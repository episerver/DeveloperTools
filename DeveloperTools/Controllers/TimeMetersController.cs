using System.Linq;
using System.Web.Mvc;
using EPiServer.Framework.Initialization;

namespace DeveloperTools.Controllers
{
    public class TimeMetersController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            var allTimers = TimeMeters.GetAllRegistered();
            return View("Index", allTimers.OrderByDescending(t => t.Counters.Values.Max(sw => sw.ElapsedMilliseconds)));
        }
    }
}
