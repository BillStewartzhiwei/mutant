using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Mutant.Core.Modules
{
    public sealed class ModuleManager
    {
        private static ModuleManager _instance;
        public static ModuleManager Instance => _instance ??= new ModuleManager();

        private readonly List<IModule> _orderedModules = new();
        private readonly Dictionary<Type, IModule> _typeMap = new();
        private readonly Dictionary<string, IModule> _nameMap = new(StringComparer.Ordinal);

        private bool _initialized;
        private bool _logLifecycle;

        private ModuleManager() { }

        public void Configure(bool logLifecycle)
        {
            _logLifecycle = logLifecycle;
        }

        public bool IsInitialized => _initialized;

        public IReadOnlyList<IModule> GetAllModules()
        {
            return _orderedModules;
        }

        public bool HasModule<T>() where T : class, IModule
        {
            return _typeMap.ContainsKey(typeof(T));
        }

        public bool TryGetModule<T>(out T module) where T : class, IModule
        {
            if (_typeMap.TryGetValue(typeof(T), out IModule value))
            {
                module = value as T;
                return module != null;
            }

            module = null;
            return false;
        }

        public T GetModule<T>() where T : class, IModule
        {
            return TryGetModule<T>(out T module) ? module : null;
        }

        public IModule GetModule(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            _nameMap.TryGetValue(name, out IModule module);
            return module;
        }

        public bool Register(IModule module)
        {
            if (module == null)
                return false;

            Type type = module.GetType();

            if (_typeMap.ContainsKey(type))
                return false;

            if (!string.IsNullOrEmpty(module.Name) && _nameMap.ContainsKey(module.Name))
                return false;

            _typeMap[type] = module;

            if (!string.IsNullOrEmpty(module.Name))
                _nameMap[module.Name] = module;

            _orderedModules.Add(module);
            _orderedModules.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            if (_initialized)
            {
                module.Init();
                Log($"Init late-registered module: {module.Name}");
            }

            return true;
        }

        public T Register<T>() where T : class, IModule, new()
        {
            if (TryGetModule<T>(out T existing))
                return existing;

            T module = new T();
            Register(module);
            return module;
        }

        public bool Unregister<T>() where T : class, IModule
        {
            return Unregister(typeof(T));
        }

        public bool Unregister(Type type)
        {
            if (type == null)
                return false;

            if (!_typeMap.TryGetValue(type, out IModule module))
                return false;

            if (_initialized)
            {
                module.Dispose();
                Log($"Dispose unregistered module: {module.Name}");
            }

            _typeMap.Remove(type);

            if (!string.IsNullOrEmpty(module.Name))
                _nameMap.Remove(module.Name);

            _orderedModules.Remove(module);
            return true;
        }

        public void AutoRegisterAttributedModules()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(SafeGetTypes)
                .Where(t =>
                    t != null &&
                    !t.IsAbstract &&
                    typeof(IModule).IsAssignableFrom(t) &&
                    t.GetCustomAttribute<AutoRegisterModuleAttribute>() != null &&
                    t.GetConstructor(Type.EmptyTypes) != null);

            foreach (Type type in types)
            {
                if (_typeMap.ContainsKey(type))
                    continue;

                if (Activator.CreateInstance(type) is IModule instance)
                    Register(instance);
            }
        }

        public void InitAll()
        {
            if (_initialized)
                return;

            _initialized = true;

            foreach (IModule module in _orderedModules)
            {
                module.Init();
                Log($"Init module: {module.Name}");
            }
        }

        public void UpdateAll()
        {
            if (!_initialized)
                return;

            foreach (IModule module in _orderedModules)
                module.Update();
        }

        public void LateUpdateAll()
        {
            if (!_initialized)
                return;

            foreach (IModule module in _orderedModules)
                module.LateUpdate();
        }

        public void FixedUpdateAll()
        {
            if (!_initialized)
                return;

            foreach (IModule module in _orderedModules)
                module.FixedUpdate();
        }

        public void DisposeAll()
        {
            if (!_initialized)
            {
                ClearRegistrations();
                return;
            }

            for (int i = _orderedModules.Count - 1; i >= 0; i--)
            {
                IModule module = _orderedModules[i];
                module.Dispose();
                Log($"Dispose module: {module.Name}");
            }

            ClearRegistrations();
            _initialized = false;
        }

        private void ClearRegistrations()
        {
            _orderedModules.Clear();
            _typeMap.Clear();
            _nameMap.Clear();
        }

        private void Log(string message)
        {
            if (_logLifecycle)
                Debug.Log("[ModuleManager] " + message);
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }
    }
}