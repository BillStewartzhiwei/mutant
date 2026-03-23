using UnityEngine;

namespace Bill.Mutant.Core
{
    [CreateAssetMenu(
        fileName = "LogConfig",
        menuName = "Mutant/Core/Log Config",
        order = 1000)]
    public class LogConfig : ScriptableObject
    {
        [Header("General")]
        public bool EnableLog = true;
        public LogLevel MinimumLevel = LogLevel.Info;

        [Header("Outputs")]
        public bool EnableConsole = true;
        public bool EnableFile = false;
        public bool EnableMemory = true;

        [Header("Details")]
        public bool IncludeStackTrace = true;
        public bool IncludeFrameCount = true;
        public bool IncludeTimestamp = true;
        public bool IncludeSource = true;
        public bool IncludeCategory = true;

        [Header("File")]
        public string FileName = "mutant-runtime.log";
        public string DirectoryName = "Logs";

        [Header("Memory")]
        [Min(10)]
        public int MaxMemoryEntries = 1000;

        [Header("Editor")]
        public bool AutoOpenViewerOnPlay = false;
        public bool ClearMemoryOnPlay = true;
    }
}
