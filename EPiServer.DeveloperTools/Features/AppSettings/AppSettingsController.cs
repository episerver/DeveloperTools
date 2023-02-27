using EPiServer.DeveloperTools.Features.Common;
using EPiServer.Licensing.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;


namespace EPiServer.DeveloperTools.Features.AppSettings
{
    public class AppSettingsController : DeveloperToolsController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AppSettingsController(IWebHostEnvironment webHostEnvironment)
        {
                _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            string webRootPath = _webHostEnvironment.ContentRootPath;
            string file = "appsettings.json";
            string[] str = null;

            string path = "";
            path = Path.Combine(webRootPath, file);

            if (System.IO.File.Exists(path))
            {
                str = System.IO.File.ReadAllLines(path);
            }

            return View(str.ToList());
        }
    }
}
