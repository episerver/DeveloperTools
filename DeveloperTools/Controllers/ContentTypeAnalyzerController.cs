using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using DeveloperTools.Models;
using EPiServer.Framework.Initialization;
using StructureMap;
using EPiServer.ServiceLocation;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;
using EPiServer.Core;
using EPiServer.Security;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DeveloperTools.Controllers
{

    public class ContentTypeAnalyzerController : DeveloperToolsController
    {

        public ActionResult Index()
        {
            return View(ServiceLocator.Current.GetInstance<ContentTypeModelRepository>().List());
        }

    }
}
