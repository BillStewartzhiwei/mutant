using System;

namespace Bill.Mutant.Core
{
    public struct LogEntry
    {
        public DateTime Timestamp;
        public LogLevel Level;
        public string Category;
        public string Message;
        public string StackTrace;
        public string Source;
        public int Frame;

        public LogEntry(
            LogLevel level,
            string category,
            string message,
            string source = null,
            string stackTrace = null,
            int frame = -1)
        {
            Timestamp = DateTime.Now;
            Level = level;
            Category = string.IsNullOrWhiteSpace(category) ? LogCategory.Core : category;
            Message = message ?? string.Empty;
            Source = source ?? string.Empty;
            StackTrace = stackTrace ?? string.Empty;
            Frame = frame;
        }
    }
}
