using EPiServer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
