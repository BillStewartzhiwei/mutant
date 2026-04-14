using Mutant.VR.Bootstrap;
using Mutant.VR.Core;
using UnityEngine;

namespace Mutant.VR.CoreBridge
{
    [DisallowMultipleComponent]
    public sealed class MutantVrCoreBridgeInstaller : MonoBehaviour
    {
        [SerializeField] private MutantVrInstaller _vrInstaller;

        public MutantVrInstaller VrInstaller
        {
            get { return _vrInstaller; }
        }

        public bool HasInstaller
        {
            get { return _vrInstaller != null; }
        }

        public MutantVrRuntime Runtime
        {
            get
            {
                return _vrInstaller != null ? _vrInstaller.Runtime : null;
            }
        }

        private void OnEnable()
        {
            if (_vrInstaller == null)
            {
                _vrInstaller = GetComponent<MutantVrInstaller>();
            }

            MutantVrCoreBridgeRegistry.Register(this);
        }

        private void OnDisable()
        {
            MutantVrCoreBridgeRegistry.Unregister(this);
        }

        public MutantVrRuntime BuildRuntime()
        {
            if (_vrInstaller == null)
            {
                Debug.LogError("[MutantVrCoreBridgeInstaller] MutantVrInstaller is missing.");
                return null;
            }

            return _vrInstaller.BuildRuntime();
        }

        public MutantVrRuntime GetOrBuildRuntime()
        {
            if (_vrInstaller == null)
            {
                Debug.LogError("[MutantVrCoreBridgeInstaller] MutantVrInstaller is missing.");
                return null;
            }

            return _vrInstaller.Runtime ?? _vrInstaller.BuildRuntime();
        }

        public bool EnsureInstalled()
        {
            var runtime = GetOrBuildRuntime();
            if (runtime == null)
            {
                return false;
            }

            return runtime.Install();
        }

        public void ShutdownRuntime()
        {
            Runtime?.Shutdown();
        }

        private void Reset()
        {
            if (_vrInstaller == null)
            {
                _vrInstaller = GetComponent<MutantVrInstaller>();
            }
        }
    }
}