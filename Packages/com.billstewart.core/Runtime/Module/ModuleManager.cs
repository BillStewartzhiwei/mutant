using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bill.Mutant.Core
{
    public class ModuleManager
    {
        private static ModuleManager _instance;
        public static ModuleManager Instance => _instance ??= new ModuleManager();

        private readonly List<IModule> _modules = new();
        private bool _initialized;

        public void Init()
        {
            if (_initialized) return;

            var modules = ScanModules();
            var sorted = ModuleDependencyResolver.Resolve(modules);

            foreach (var info in sorted)
            {
                var module = (IModule)Activator.CreateInstance(info.ModuleType);
                _modules.Add(module);
            }

            foreach (var m in _modules)
            {
                m.Init();
            }

            _initialized = true;
        }

        public void Update(float dt)
        {
            if (!_initialized) return;
            foreach (var m in _modules) m.Update(dt);
        }

        private List<ModuleInfo> ScanModules()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IModule).IsAssignableFrom(t)
                            && !t.IsAbstract
                            && t.GetCustomAttribute<ModuleAttribute>() != null)
                .Select(t => new ModuleInfo(
                    t,
                    t.GetCustomAttribute<ModuleAttribute>().Order,
                    t.GetCustomAttributes<RequireAttribute>().Select(r => r.ModuleType).ToList()
                ))
                .ToList();
        }
    }
}
