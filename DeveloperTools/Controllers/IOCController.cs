using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using DeveloperTools.Models;
using EPiServer.Framework.Initialization;
using StructureMap;

namespace DeveloperTools.Controllers
{

    public class IOCController : DeveloperToolsController
    {
        public ActionResult Index()
        {

            var ie = ((InitializationEngine)typeof(EPiServer.Framework.Initialization.InitializationModule).GetField("_engine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null));
            IContainer container = (IContainer)ie.GetType().GetProperty("Container", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ie, new object[0]);

            List<IOCEntry> iocEntries = new List<IOCEntry>();
            List<string> typeErrors = new List<string>();

            foreach (var plugin in container.Model.PluginTypes)
            {
                try
                {
                    Type defaultType = plugin.Default != null ? plugin.Default.ReturnedType : null;
                    if (plugin.Default != null && defaultType == null)
                    {
                        defaultType = container.GetInstance(plugin.Default.PluginType, plugin.Default.Name).GetType();
                    }

                    foreach (var entry in plugin.Instances.Where(i => i != null))
                    {
                        Type concreteType = entry.ReturnedType;
                        if (concreteType == null && entry.PluginType.ContainsGenericParameters == false)
                        {
                            concreteType = container.GetInstance(entry.PluginType, entry.Name).GetType();
                        }

                        iocEntries.Add(new IOCEntry()
                        {
                            PluginType = entry.PluginType == null ? "null" : entry.PluginType.FullName + "," + entry.PluginType.Assembly.FullName,
                            ConcreteType = concreteType == null ? "null" : concreteType.FullName + "," + concreteType.Assembly.FullName,
                            Scope = plugin.Lifecycle.ToString(),
                            IsDefault = defaultType == concreteType
                        });
                    }
                }
                catch (Exception ex)
                {
                    typeErrors.Add("Failed to get type " + plugin.PluginType.FullName + ", reason:" + ex.Message);
                }
            }

            var model = new IOCModel() { IOCEntries = iocEntries, LoadingErrors = typeErrors };

            return View(model);
        }
    }
}
