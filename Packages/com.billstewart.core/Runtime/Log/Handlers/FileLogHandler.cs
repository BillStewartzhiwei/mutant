using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Bill.Mutant.Core
{
    public class FileLogHandler : ILogHandler
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public string FilePath => _filePath;

        public FileLogHandler(LogConfig config)
        {
            string dir = Path.Combine(Application.persistentDataPath, config.DirectoryName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            _filePath = Path.Combine(dir, config.FileName);
            WriteLine($"========== Mutant Log Started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==========");
        }

        public void Handle(LogEntry entry)
        {
            var sb = new StringBuilder();
            sb.Append("[Mutant]");
            sb.Append($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}]");
            sb.Append($"[{entry.Level}]");
            sb.Append($"[{entry.Category}]");

            if (!string.IsNullOrEmpty(entry.Source))
                sb.Append($"[{entry.Source}]");

            if (entry.Frame >= 0)
                sb.Append($"[F:{entry.Frame}]");

            sb.Append(" ");
            sb.Append(entry.Message);

            if (!string.IsNullOrEmpty(entry.StackTrace))
            {
                sb.AppendLine();
                sb.Append(entry.StackTrace);
            }

            WriteLine(sb.ToString());
        }

        public void Flush()
        {
        }

        public void Dispose()
        {
            WriteLine($"========== Mutant Log Closed {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==========");
        }

        private void WriteLine(string content)
        {
            lock (_lock)
            {
                File.AppendAllText(_filePath, content + Environment.NewLine, Encoding.UTF8);
            }
        }
    }
}
