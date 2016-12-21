using System;
using System.Web.Mvc;
using EPiServer.Security;

namespace DeveloperTools.Controllers
{
    public class DeveloperToolsController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if(!PrincipalInfo.HasAdminAccess)
            {
                throw new UnauthorizedAccessException();
            }

            base.OnAuthorization(filterContext);
        }
    }
}
