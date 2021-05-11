using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeveloperTools.Core;
using DeveloperTools.Models;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperTools.Controllers
{
    public class ModuleDependenciesController : DeveloperToolsController
    {
        private readonly Dictionary<string, int> _assemblyGroups = new Dictionary<string, int>();
        private readonly IInitializationEngine _engine;

        public ModuleDependenciesController(IInitializationEngine engine)
        {
            _engine = engine;
        }

        public ActionResult Index(bool showAll = false)
        {
            var sortedModules = GetSortedModules();

            var modules = from module in sortedModules
                          let asmName = module.Type.Assembly.GetName().Name + ".dll"
                          select new ModuleInfo
                                 {
                                     Id = module.Type.FullName,
                                     ModuleType = module.Type,
                                     Label = module.Type.Name,
                                     Title = $"Name: {module.Type.Name}<br>Type: {(typeof(IConfigurableModule).IsAssignableFrom(module.Type) ? "Configurable" : "Initializable")}<br>Namespace: {module.Type.Namespace}<br>Assembly: {asmName}",
                                     Group = GetAssemblyGroup(asmName)
                                 };

            var dependencies = sortedModules.Where(_ => _.Dependencies.Count > 0).SelectMany(_ => _.Dependencies,
                                                                                             (from, to) => new ModuleDependency
                                                                                                           {
                                                                                                               From = from.Type.FullName,
                                                                                                               To = to.FullName
                                                                                                           });

            if(!showAll)
            {
                dependencies = dependencies.Where(_ => !_.From.StartsWith("EPiServer"));
                modules = modules.Where(m => !m.ModuleType.FullName.StartsWith("EPiServer"))
                                 .Union(modules.Where(m => dependencies.Any(d => d.To == m.ModuleType.FullName)))
                                 .DistinctBy(_ => _.Id).ToList();
            }

            var model = new ModuleDependencyViewModel
                            {
                                Nodes = modules.ToList(),
                                Links = dependencies.ToList(),
                                ShowAll = showAll
                            };

            WeightModulesByDependants(model);

            return View(model);
        }

        private void WeightModulesByDependants(ModuleDependencyViewModel model)
        {
            // process sizes of the modules
            var incomingDeps = model.Links.GroupBy(_ => _.To)
                                    .Select(group => new
                                                     {
                                                         Name = group.Key,
                                                         Count = group.Count()
                                                     });

            foreach (var incomingDep in incomingDeps)
            {
                var m = model.Nodes.First(_ => _.Id == incomingDep.Name);
                m.Size = m.Size + incomingDep.Count * 2;
            }
        }

        private List<ExtractedModuleInfo> GetSortedModules()
        {
            var sortedModules = _engine.GetType().GetField("_dependencySortedList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_engine) as IList;
            var result = new List<ExtractedModuleInfo>();

            // some nasty dirty internal business to get sorted module list with dependencies
            foreach (var module in sortedModules)
            {
                var t = module.GetType();

                // we cannot cast to `ModuleNode` as this is internal type. we have to blindly invoke property from the underlying type
                var moduleType = t.GetProperty("ModuleType", BindingFlags.Instance | BindingFlags.Public).GetValue(module) as Type;
                var dependencies = t.GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.Public).GetValue(module) as IEnumerable;

                var dependencyTypes = (from object dep in dependencies
                                       select dep as Type);

                result.Add(new ExtractedModuleInfo
                           {
                               Type = moduleType,
                               Dependencies = dependencyTypes.ToList()
                           });
            }

            return result;
        }

        private int GetAssemblyGroup(string assemblyName)
        {
            if(!_assemblyGroups.Any())
            {
                _assemblyGroups.Add(assemblyName, 1);
                return 1;
            }

            if(_assemblyGroups.ContainsKey(assemblyName))
                return _assemblyGroups[assemblyName];

            var max = _assemblyGroups.Max(_ => _.Value);
            _assemblyGroups.Add(assemblyName, max + 1);

            return max;
        }
    }
}
