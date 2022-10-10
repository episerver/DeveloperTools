using System.Linq;
using DeveloperTools.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EPiServer.DeveloperTools.Features.IoC
{
    public class IOCController : DeveloperToolsController
    {
        private readonly ServiceCollectionClosure _closure;

        public IOCController(ServiceCollectionClosure closure)
        {
            _closure = closure;
        }

        public ActionResult Index()
        {
            var model = new IOCModel();

            var services = _closure.Services;

            model.IOCEntries = services.Select(s => new IOCEntry
            {
                PluginType = s.ServiceType.FullName,
                ConcreteType = s.ImplementationType?.FullName,
                ImplementationFactory = s.ImplementationFactory?.Method.ToString(),
                ImplementationInstance = s.ImplementationInstance?.GetType().FullName,
                Scope = s.Lifetime.ToString()
            });

            return View(model);
        }
    }
}
