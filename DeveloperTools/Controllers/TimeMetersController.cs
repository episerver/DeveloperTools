using System.Linq;
using EPiServer.Cms.Shell.UI.Controllers.Internal;
using EPiServer.Framework.Initialization;
using Microsoft.AspNetCore.Mvc;

namespace EPiServer.DeveloperTools.Controllers;

public class TimeMetersController : BaseController
{
    [HttpGet]
    public ActionResult Index()
    {
        var allTimers = TimeMeters.GetAllRegistered();
        return View("Index", allTimers.OrderByDescending(t => t.Counters.Values.Max(sw => sw.ElapsedMilliseconds)));
    }
}
