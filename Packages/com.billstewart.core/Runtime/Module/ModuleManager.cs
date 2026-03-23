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

            foreach (var m in _modules)
            {
                m.Update(dt);
            }
        }

        public void Dispose()
        {
            if (!_initialized) return;

            for (int i = _modules.Count - 1; i >= 0; i--)
            {
                _modules[i].Dispose();
            }

            _modules.Clear();
            _initialized = false;
        }

        public T GetModule<T>() where T : class, IModule
        {
            return _modules.OfType<T>().FirstOrDefault();
        }

        private List<ModuleInfo> ScanModules()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetLoadableTypes)
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

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}