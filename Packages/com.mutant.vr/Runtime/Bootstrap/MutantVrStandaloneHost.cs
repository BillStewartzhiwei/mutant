using Mutant.VR.Core;
using UnityEngine;

namespace Mutant.VR.Bootstrap
{
    [DisallowMultipleComponent]
    public sealed class MutantVrStandaloneHost : MonoBehaviour
    {
        [SerializeField] private MutantVrInstaller _installer;
        [SerializeField] private bool _installOnAwake = true;
        [SerializeField] private bool _tickInUpdate = true;
        [SerializeField] private bool _shutdownOnDestroy = true;

        private MutantVrRuntime _runtime;

        private void Awake()
        {
            if (_installer == null)
            {
                _installer = GetComponent<MutantVrInstaller>();
            }

            if (_installer == null)
            {
                Debug.LogError("[MutantVrStandaloneHost] MutantVrInstaller is missing.");
                enabled = false;
                return;
            }

            _runtime = _installer.BuildRuntime();

            if (_installOnAwake)
            {
                _runtime.Install();
            }
        }

        private void Update()
        {
            if (!_tickInUpdate || _runtime == null)
            {
                return;
            }

            _runtime.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (_shutdownOnDestroy)
            {
                _runtime?.Shutdown();
            }
        }

        private void Reset()
        {
            if (_installer == null)
            {
                _installer = GetComponent<MutantVrInstaller>();
            }
        }
    }
}
