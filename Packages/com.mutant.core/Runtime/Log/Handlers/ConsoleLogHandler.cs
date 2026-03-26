using UnityEngine;

namespace Bill.Mutant.Core
{
    public class ConsoleLogHandler : ILogHandler
    {
        public void Handle(LogEntry entry)
        {
	        
#if UNITY_EDITOR
            string prefix = FormatPrefix(entry);
            string content = $"{prefix}{entry.Message}";

            switch (entry.Level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Info:
                    Debug.Log(content);
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(content);
                    break;

                case LogLevel.Error:
                case LogLevel.Fatal:
                    Debug.LogError(content);
                    break;
            }
#endif

        }

        public void Flush()
        {
        }

        public void Dispose()
        {
        }

        private string FormatPrefix(LogEntry entry)
        {
            string time = entry.Timestamp.ToString("HH:mm:ss.fff");
            string source = string.IsNullOrEmpty(entry.Source) ? string.Empty : $"[{entry.Source}]";
            string frame = entry.Frame >= 0 ? $"[F:{entry.Frame}]" : string.Empty;
            return $"[Mutant][{time}][{entry.Level}][{entry.Category}]{source}{frame} ";
        }
    }
}
