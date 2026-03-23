using System;
using System.Collections.Generic;

namespace Bill.Mutant.Core
{
    public class InMemoryLogHandler : ILogHandler
    {
        private readonly List<LogEntry> _entries = new List<LogEntry>();
        private readonly object _lock = new object();
        private readonly int _maxEntries;

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _entries.Count;
                }
            }
        }

        public InMemoryLogHandler(int maxEntries = 1000)
        {
            _maxEntries = Math.Max(10, maxEntries);
        }

        public void Handle(LogEntry entry)
        {
            lock (_lock)
            {
                _entries.Add(entry);

                int overflow = _entries.Count - _maxEntries;
                if (overflow > 0)
                {
                    _entries.RemoveRange(0, overflow);
                }
            }
        }

        public IReadOnlyList<LogEntry> GetEntriesSnapshot()
        {
            lock (_lock)
            {
                return _entries.ToArray();
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _entries.Clear();
            }
        }

        public void Flush()
        {
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
