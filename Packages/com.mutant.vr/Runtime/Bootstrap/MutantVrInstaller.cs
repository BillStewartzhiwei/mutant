using Mutant.VR.Config;
using Mutant.VR.Contracts;
using Mutant.VR.Core;
using Mutant.VR.Diagnostics;
using Mutant.VR.Rig;
using UnityEngine;

namespace Mutant.VR.Bootstrap
{
    [DisallowMultipleComponent]
    public sealed class MutantVrInstaller : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private MutantVrSettings _settings;

        [Header("Scene References")]
        [SerializeField] private MutantVrRigRoot _rigRoot;
        [SerializeField] private MonoBehaviour _platformAdapterBehaviour;

        private MutantVrContext _runtimeContext;
        private MutantVrRuntime _runtime;

        public MutantVrContext RuntimeContext => _runtimeContext;
        public MutantVrRuntime Runtime => _runtime;
        public MutantVrRuntime RuntimeModule => _runtime;

        [ContextMenu("Mutant/VR/Rebuild Runtime")]
        public void RebuildRuntime()
        {
            _runtimeContext = BuildRuntimeContext();
            _runtime = new MutantVrRuntime(_runtimeContext);
        }

        [ContextMenu("Mutant/VR/Install Runtime")]
        public void InstallRuntime()
        {
            if (_runtime == null)
            {
                RebuildRuntime();
            }

            _runtime.Install();
        }

        [ContextMenu("Mutant/VR/Tick Runtime Once")]
        public void TickRuntimeOnce()
        {
            if (_runtime == null)
            {
                RebuildRuntime();
            }

            _runtime.Tick(Time.deltaTime);
        }

        [ContextMenu("Mutant/VR/Shutdown Runtime")]
        public void ShutdownRuntime()
        {
            _runtime?.Shutdown();
        }

        public MutantVrContext BuildRuntimeContext()
        {
            IMutantVrPlatformAdapter adapter = ResolvePlatformAdapter();
            MutantVrRigReferences rigReferences = _rigRoot != null
                ? _rigRoot.BuildRigReferences()
                : new MutantVrRigReferences();

            var diagnostics = new MutantVrUnityDiagnostics(_settings);
            var context = new MutantVrContext();
            context.Configure(_settings, rigReferences, adapter, diagnostics);
            return context;
        }

        public MutantVrRuntime BuildRuntime()
        {
            _runtimeContext = BuildRuntimeContext();
            _runtime = new MutantVrRuntime(_runtimeContext);
            return _runtime;
        }

        private IMutantVrPlatformAdapter ResolvePlatformAdapter()
        {
            if (_platformAdapterBehaviour is IMutantVrPlatformAdapter serializedAdapter)
            {
                return serializedAdapter;
            }

            MonoBehaviour[] localBehaviours = GetComponents<MonoBehaviour>();
            for (int i = 0; i < localBehaviours.Length; i++)
            {
                if (localBehaviours[i] is IMutantVrPlatformAdapter foundAdapter)
                {
                    _platformAdapterBehaviour = localBehaviours[i];
                    return foundAdapter;
                }
            }

            MonoBehaviour[] childBehaviours = GetComponentsInChildren<MonoBehaviour>(true);
            for (int i = 0; i < childBehaviours.Length; i++)
            {
                if (childBehaviours[i] is IMutantVrPlatformAdapter foundAdapter)
                {
                    _platformAdapterBehaviour = childBehaviours[i];
                    return foundAdapter;
                }
            }

            Debug.LogWarning("[MutantVrInstaller] No IMutantVrPlatformAdapter found.");
            return null;
        }

        private void Reset()
        {
            if (_rigRoot == null)
            {
                _rigRoot = GetComponentInChildren<MutantVrRigRoot>(true);
            }
        }
    }
}
