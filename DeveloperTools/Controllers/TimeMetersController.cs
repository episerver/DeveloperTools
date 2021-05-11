using System.Linq;
using EPiServer.Framework.Initialization;
using Microsoft.AspNetCore.Mvc;

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
