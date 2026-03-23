namespace Bill.Mutant.Core
{
    public interface ILogHandler
    {
        void Handle(LogEntry entry);
        void Flush();
        void Dispose();
    }
}
