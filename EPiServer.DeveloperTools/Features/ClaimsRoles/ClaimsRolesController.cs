using EPiServer.DeveloperTools.Features.Common;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace EPiServer.DeveloperTools.Features.ClaimsRoles
{
    public class ClaimsRolesController : DeveloperToolsController
    {
        public IActionResult Index()
        {
            var user = base.User;
            var claims = user.Identities.First().Claims.ToList();

            var model = new ClaimsRolesModel { Roles = claims };

            return View(model);
        }
    }
}
