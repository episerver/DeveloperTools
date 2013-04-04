using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Shell.Gadgets;
using System.Web.Mvc;
using EPiServer.Framework.Initialization;
using EPiServer.Shell.Web;

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
