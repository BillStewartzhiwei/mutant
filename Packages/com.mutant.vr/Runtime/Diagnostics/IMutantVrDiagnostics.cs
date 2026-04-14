namespace Mutant.VR.Diagnostics
{
    public interface IMutantVrDiagnostics
    {
        void LogVerbose(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
