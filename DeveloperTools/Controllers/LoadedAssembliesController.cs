using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using DeveloperTools.Models;
using EPiServer.Framework.Initialization;
using StructureMap;
using System.Reflection;
using System.Diagnostics;

namespace DeveloperTools.Controllers
{
    public class LoadedAssembliesController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            var assemblies = new List<AssemblyInfo>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string fileVersion = null;
                string location = assembly.IsDynamic ? "dynamic" : String.IsNullOrEmpty(assembly.Location) ? new Uri(assembly.CodeBase).LocalPath : assembly.Location;

                if (!assembly.IsDynamic)
                {
                    AssemblyFileVersionAttribute fileVersionAttribute = (AssemblyFileVersionAttribute)assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false).FirstOrDefault();
                    if (fileVersionAttribute != null)
                    {
                        fileVersion = fileVersionAttribute.Version;
                    }

                    if (fileVersion == null)
                    {
                        FileVersionInfo versionInfo = assembly.IsDynamic ? null : FileVersionInfo.GetVersionInfo(location);
                        if (versionInfo != null)
                        {
                            fileVersion = versionInfo.FileVersion;
                        }
                    }
                }

                assemblies.Add(new AssemblyInfo()
                {
                    Name = assembly.FullName,
                    AssemblyVersion = assembly.GetName().Version.ToString(),
                    FileVersion = fileVersion ?? assembly.GetName().Version.ToString(),
                    Location = assembly.IsDynamic ? "dynamic" : location
                });
            }

            return View(new AssembliesModel() { Assemblies = assemblies });
        }
    }
}
