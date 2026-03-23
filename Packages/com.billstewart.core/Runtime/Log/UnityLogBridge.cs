using UnityEngine;

namespace Bill.Mutant.Core
{
    public static class UnityLogBridge
    {
        private static bool _installed;
        private static bool _suppressCallback;

        public static void Install()
        {
            if (_installed) return;
            Application.logMessageReceived += OnLogMessageReceived;
            _installed = true;
        }

        public static void Uninstall()
        {
            if (!_installed) return;
            Application.logMessageReceived -= OnLogMessageReceived;
            _installed = false;
        }

        private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (_suppressCallback) return;

            try
            {
                _suppressCallback = true;

                switch (type)
                {
                    case LogType.Warning:
                        LogManager.Instance.Log(new LogEntry(LogLevel.Warning, LogCategory.Core, condition, "Unity", stackTrace, Time.frameCount));
                        break;

                    case LogType.Error:
                    case LogType.Assert:
                    case LogType.Exception:
                        LogManager.Instance.Log(new LogEntry(LogLevel.Error, LogCategory.Core, condition, "Unity", stackTrace, Time.frameCount));
                        break;

                    default:
                        LogManager.Instance.Log(new LogEntry(LogLevel.Debug, LogCategory.Core, condition, "Unity", stackTrace, Time.frameCount));
                        break;
                }
            }
            finally
            {
                _suppressCallback = false;
            }
        }
    }
}
