namespace Bill.Mutant.Core
{
    public interface ILogger
    {
        void Trace(string message, string category = null, string source = null);
        void Debug(string message, string category = null, string source = null);
        void Info(string message, string category = null, string source = null);
        void Warning(string message, string category = null, string source = null);
        void Error(string message, string category = null, string source = null);
        void Fatal(string message, string category = null, string source = null);
        void Log(LogEntry entry);
    }
}
