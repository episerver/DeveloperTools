using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DeveloperTools.Models;

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
                var location = assembly.IsDynamic ? "dynamic" : string.IsNullOrEmpty(assembly.Location) ? new Uri(assembly.CodeBase).LocalPath : assembly.Location;

                if(!assembly.IsDynamic)
                {
                    var fileVersionAttribute = (AssemblyFileVersionAttribute) assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false).FirstOrDefault();
                    if(fileVersionAttribute != null)
                    {
                        fileVersion = fileVersionAttribute.Version;
                    }

                    if(fileVersion == null)
                    {
                        var versionInfo = assembly.IsDynamic ? null : FileVersionInfo.GetVersionInfo(location);
                        if(versionInfo != null)
                        {
                            fileVersion = versionInfo.FileVersion;
                        }
                    }
                }

                assemblies.Add(new AssemblyInfo
                               {
                                   Name = assembly.FullName,
                                   AssemblyVersion = assembly.GetName().Version.ToString(),
                                   FileVersion = fileVersion ?? assembly.GetName().Version.ToString(),
                                   Location = assembly.IsDynamic ? "dynamic" : location
                               });
            }

            return View(new AssembliesModel { Assemblies = assemblies });
        }
    }
}
