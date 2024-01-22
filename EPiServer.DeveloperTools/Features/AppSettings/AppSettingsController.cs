using EPiServer.DeveloperTools.Features.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace EPiServer.DeveloperTools.Features.AppSettings
{
    public class AppSettingsController : DeveloperToolsController
    {
        private readonly IConfiguration _configuration;

        public AppSettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            var root = (IConfigurationRoot)_configuration;
            var model = new AppSettingsModel { FinalValues = root.GetDebugView() };

            return View(model);
        }
    }
}
