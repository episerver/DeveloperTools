using EPiServer.Cms.Shell.UI.Controllers.Internal;
using Microsoft.AspNetCore.Mvc;

namespace EPiServer.DeveloperTools.Controllers;

public class DefaultController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
