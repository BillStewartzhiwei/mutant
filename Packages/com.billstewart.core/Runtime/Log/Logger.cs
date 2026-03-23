namespace Bill.Mutant.Core
{
    public static class Logger
    {
        public static bool EnableLog
        {
            get => LogManager.Instance.Config != null && LogManager.Instance.Config.EnableLog;
            set => LogManager.Instance.SetEnableLog(value);
        }

        public static void Init(LogConfig config = null)
        {
            LogManager.Instance.Init(config);
        }

        public static void Shutdown()
        {
            LogManager.Instance.Shutdown();
        }

        public static System.Collections.Generic.IReadOnlyList<LogEntry> GetMemoryEntries()
        {
            return LogManager.Instance.GetMemoryEntries();
        }

        public static void ClearMemory()
        {
            LogManager.Instance.ClearMemory();
        }

        public static void Log(string msg)
        {
            LogManager.Instance.Info(msg, LogCategory.Core);
        }

        public static void Trace(string message)
        {
            LogManager.Instance.Trace(message, LogCategory.Core);
        }

        public static void Debug(string message)
        {
            LogManager.Instance.Debug(message, LogCategory.Core);
        }

        public static void Info(string message)
        {
            LogManager.Instance.Info(message, LogCategory.Core);
        }

        public static void Warning(string message)
        {
            LogManager.Instance.Warning(message, LogCategory.Core);
        }

        public static void Error(string message)
        {
            LogManager.Instance.Error(message, LogCategory.Core);
        }

        public static void Fatal(string message)
        {
            LogManager.Instance.Fatal(message, LogCategory.Core);
        }

        public static void Trace(string category, string message, string source = null)
        {
            LogManager.Instance.Trace(message, category, source);
        }

        public static void Debug(string category, string message, string source = null)
        {
            LogManager.Instance.Debug(message, category, source);
        }

        public static void Info(string category, string message, string source = null)
        {
            LogManager.Instance.Info(message, category, source);
        }

        public static void Warning(string category, string message, string source = null)
        {
            LogManager.Instance.Warning(message, category, source);
        }

        public static void Error(string category, string message, string source = null)
        {
            LogManager.Instance.Error(message, category, source);
        }

        public static void Fatal(string category, string message, string source = null)
        {
            LogManager.Instance.Fatal(message, category, source);
        }
    }
}