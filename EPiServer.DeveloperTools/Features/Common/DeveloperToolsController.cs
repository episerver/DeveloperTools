using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPiServer.DeveloperTools.Features.Common;

[Authorize(Constants.PolicyName)]
public class DeveloperToolsController : Controller { }
