using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DeveloperTools.Models;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

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
            var sortedModules = _engine.GetType().GetField("_dependencySortedList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_engine) as IList;
            var l = new List<ExtractedModuleInfo>();

            // some dirty internals toget to sorted module list
            foreach (var m in sortedModules)
            {
                var t = m.GetType();
                var tt = t.GetProperty("ModuleType", BindingFlags.Instance | BindingFlags.Public).GetValue(m) as Type;
                var deps = t.GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.Public).GetValue(m) as IEnumerable;

                var depps = (from object dep in deps
                             select dep as Type).ToList();

                l.Add(new ExtractedModuleInfo
                      {
                          Type = tt,
                          Dependencies = depps
                      });
            }

            var modules = (from module in l
                           let asmName = module.Type.Assembly.GetName().Name + ".dll"
                           select new ModuleInfo
                                  {
                                      Id = module.Type.FullName,
                                      ModuleType = module.Type,
                                      Label = module.Type.Name,
                                      Title =
                                          $"Name: {module.Type.Name}<br>Type: {(typeof(IConfigurableModule).IsAssignableFrom(module.Type) ? "Configurable" : "Initializable")}<br>Namespace: {module.Type.Namespace}<br>Assembly: {asmName}",
                                      Group = GetAssemblyGroup(asmName)
                                  }).ToList();

            var dependencies = l.Where(_ => _.Dependencies.Count > 0)
                                .SelectMany(_ => _.Dependencies,
                                            (_, t) => new ModuleDependency
                                                      {
                                                          From = _.Type.FullName,
                                                          To = t.FullName
                                                      });

            if(!showAll)
            {
                dependencies = dependencies.Where(_ => !_.From.StartsWith("EPiServer"));
                modules = Enumerable.Union(modules.Where(m => !m.ModuleType.FullName.StartsWith("EPiServer")),
                                           modules.Where(m => dependencies.Any(d => d.To == m.ModuleType.FullName)),
                                           new ModuleListComparer()).ToList();
            }

            var model = new ModuleDependencyViewModel
                            {
                                Nodes = modules,
                                Links = dependencies,
                                ShowAll = showAll
                            };

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

            return View(model);
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

    public class ModuleListComparer : IEqualityComparer<ModuleInfo>
    {
        public bool Equals(ModuleInfo x, ModuleInfo y)
        {
            if(x == null)
                return false;

            if(y == null)
                return false;

            return x.Id.ToUpper() == y.Id.ToUpper();
        }

        public int GetHashCode(ModuleInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}