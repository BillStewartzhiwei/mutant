using Mutant.VR.Config;
using UnityEngine;

namespace Mutant.VR.Diagnostics
{
    public sealed class MutantVrUnityDiagnostics : IMutantVrDiagnostics
    {
        private readonly MutantVrSettings _settings;

        public MutantVrUnityDiagnostics(MutantVrSettings settings)
        {
            _settings = settings;
        }

        public void LogVerbose(string message)
        {
            if (_settings != null && !_settings.EnableVerboseLogging)
            {
                return;
            }

            Debug.Log($"[MutantVr] {message}");
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning($"[MutantVr] {message}");
        }

        public void LogError(string message)
        {
            Debug.LogError($"[MutantVr] {message}");
        }
    }
}
