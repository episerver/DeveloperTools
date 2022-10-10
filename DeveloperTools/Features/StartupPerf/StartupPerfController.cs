using System.Collections.Generic;
using System.Linq;
using EPiServer.Cms.Shell.UI.Controllers.Internal;
using EPiServer.Framework.Initialization;
using Microsoft.AspNetCore.Mvc;

namespace EPiServer.DeveloperTools.Features.StartupPerf;

public class StartupPerfController : BaseController
{
    [HttpGet]
    public ActionResult Index()
    {
        var allTimers = TimeMeters.GetAllRegistered();

        // flatten the list
        var result = new List<TimeMetersModel>();
        result.AddRange(ConvertToModel(allTimers));

        return View("Index", result.OrderByDescending(t => t.ElapsedMilliseconds));
    }

    private IEnumerable<TimeMetersModel> ConvertToModel(IEnumerable<TimeMeters> allTimers)
    {
        foreach (var timer in allTimers)
        {
            foreach (var counter in timer.Counters)
            {
                yield return new TimeMetersModel
                {
                    AssemblyName = timer.Owner.Assembly.GetName().Name,
                    TypeName = timer.Owner.Name,
                    MethodName = counter.Key,
                    ElapsedMilliseconds = counter.Value.ElapsedMilliseconds
                };
            }
        }
    }
}
