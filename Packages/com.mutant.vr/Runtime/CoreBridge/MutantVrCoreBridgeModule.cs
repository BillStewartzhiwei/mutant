using System;
using System.Collections.Generic;
using Mutant.Core;
using Mutant.VR.Core;
using UnityEngine;

namespace Mutant.VR.CoreBridge
{
    public sealed class MutantVrCoreBridgeModule : ModuleBase, IMutantUpdatable
    {
        private MutantVrCoreBridgeInstaller _bridgeInstaller;
        private MutantVrRuntime _runtime;

        private bool _isInitialized;
        private bool _isStarted;
        private bool _hasLoggedWaitingInstaller;
        private bool _hasLoggedRuntimeAttached;

        public override string ModuleId
        {
            get { return "Mutant.VR"; }
        }

        public override MutantBootPhase BootPhase
        {
            get { return MutantBootPhase.Platform; }
        }

        public override int Order
        {
            get { return 10; }
        }

        public override IReadOnlyList<string> Dependencies
        {
            get { return new[] { "Mutant.Log" }; }
        }

        public bool HasRuntime
        {
            get { return _runtime != null; }
        }

        public override void OnRegister()
        {
            Debug.Log("[Mutant.VR.CoreBridge] OnRegister");
        }

        public override void OnInit()
        {
            _isInitialized = true;
            _isStarted = false;
            _bridgeInstaller = null;
            _runtime = null;
            _hasLoggedWaitingInstaller = false;
            _hasLoggedRuntimeAttached = false;

            Debug.Log("[Mutant.VR.CoreBridge] OnInit");
        }

        public override void OnStart()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Mutant.VR.CoreBridge cannot start before initialization.");
            }

            _isStarted = true;
            Debug.Log("[Mutant.VR.CoreBridge] OnStart");
        }

        public override void OnStop()
        {
            if (!_isStarted)
            {
                return;
            }

            ShutdownAttachedRuntime();

            _isStarted = false;
            Debug.Log("[Mutant.VR.CoreBridge] OnStop");
        }

        public override void OnDispose()
        {
            ShutdownAttachedRuntime();

            _isInitialized = false;
            _isStarted = false;
            _bridgeInstaller = null;
            _runtime = null;

            Debug.Log("[Mutant.VR.CoreBridge] OnDispose");
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isStarted)
            {
                return;
            }

            if (_runtime == null)
            {
                TryAttachRuntime();
                return;
            }

            _runtime.Tick(deltaTime);
        }

        private void TryAttachRuntime()
        {
            if (_bridgeInstaller == null)
            {
                if (!MutantVrCoreBridgeRegistry.TryGetPrimary(out _bridgeInstaller))
                {
                    if (!_hasLoggedWaitingInstaller)
                    {
                        Debug.LogWarning("[Mutant.VR.CoreBridge] Waiting for active MutantVrCoreBridgeInstaller in scene.");
                        _hasLoggedWaitingInstaller = true;
                    }

                    return;
                }
            }

            _runtime = _bridgeInstaller.GetOrBuildRuntime();
            if (_runtime == null)
            {
                return;
            }

            bool installSuccess = _bridgeInstaller.EnsureInstalled();
            if (!installSuccess)
            {
                Debug.LogWarning("[Mutant.VR.CoreBridge] Runtime install failed, will retry on next update.");
                _runtime = null;
                return;
            }

            if (!_hasLoggedRuntimeAttached)
            {
                Debug.Log("[Mutant.VR.CoreBridge] Runtime attached successfully.");
                _hasLoggedRuntimeAttached = true;
            }
        }

        private void ShutdownAttachedRuntime()
        {
            if (_bridgeInstaller != null)
            {
                _bridgeInstaller.ShutdownRuntime();
            }
            else if (_runtime != null)
            {
                _runtime.Shutdown();
            }

            _runtime = null;
            _bridgeInstaller = null;
            _hasLoggedWaitingInstaller = false;
            _hasLoggedRuntimeAttached = false;
        }
    }
}