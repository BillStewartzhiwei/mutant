using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mutant.Core
{
    /// <summary>
    /// Mutant 启动总入口。
    /// 当前版本先只做启动编排，不接 PlayerLoop。
    /// 你应该从唯一入口调用 Boot(...)，而不是让各模块自己抢 Unity 生命周期。
    /// </summary>
    public static class MutantRuntimeBootstrap
    {
        private static bool _isBooted;
        private static ModuleRegistry _registry;
        private static ModuleManager _moduleManager;

        public static bool IsBooted
        {
            get { return _isBooted; }
        }

        public static ModuleRegistry Registry
        {
            get { return _registry; }
        }

        public static ModuleManager ModuleManager
        {
            get { return _moduleManager; }
        }

        public static ModuleManager Boot(params IMutantModule[] modules)
        {
            return Boot((IEnumerable<IMutantModule>)modules);
        }

        public static ModuleManager Boot(IEnumerable<IMutantModule> modules)
        {
            if (_isBooted)
            {
                return _moduleManager;
            }

            if (modules == null)
            {
                throw new ArgumentNullException(nameof(modules));
            }

            _registry = new ModuleRegistry();

            foreach (var module in modules)
            {
                _registry.Register(module);
            }

            var resolver = new DependencyResolver();
            var resolutionResult = resolver.Resolve(_registry.GetAutoStartDescriptors());

            var planner = new StartupPlanner();
            var startupPlan = planner.Build(resolutionResult);

            _moduleManager = new ModuleManager(_registry, startupPlan);

            Debug.Log("[Mutant] Boot begin.");

            _moduleManager.ExecuteRegister();
            _moduleManager.ExecuteInitialize();
            _moduleManager.ExecuteStart();

            _isBooted = true;

            Debug.Log("[Mutant] Boot success.");
            return _moduleManager;
        }

        public static void Shutdown()
        {
            if (!_isBooted || _moduleManager == null)
            {
                return;
            }

            Debug.Log("[Mutant] Shutdown begin.");

            _moduleManager.ExecuteStop();
            _moduleManager.ExecuteDispose();

            _moduleManager = null;
            _registry = null;
            _isBooted = false;

            Debug.Log("[Mutant] Shutdown complete.");
        }
    }
}