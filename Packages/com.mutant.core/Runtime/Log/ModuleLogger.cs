namespace Bill.Mutant.Core
{
    public sealed class ModuleLogger
    {
        private readonly string _category;
        private readonly string _source;

        public ModuleLogger(string category, string source)
        {
            _category = category;
            _source = source;
        }

        public void Trace(string message) => Logger.Trace(_category, message, _source);
        public void Debug(string message) => Logger.Debug(_category, message, _source);
        public void Info(string message) => Logger.Info(_category, message, _source);
        public void Warning(string message) => Logger.Warning(_category, message, _source);
        public void Error(string message) => Logger.Error(_category, message, _source);
        public void Fatal(string message) => Logger.Fatal(_category, message, _source);
    }
}
