using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bill.Mutant.Core
{
    public class LogManager : ILogger
    {
        private static LogManager _instance;
        public static LogManager Instance => _instance ??= new LogManager();

        private readonly List<ILogHandler> _handlers = new List<ILogHandler>();
        private LogConfig _config;
        private bool _initialized;
        private InMemoryLogHandler _memoryHandler;

        public LogConfig Config => _config;
        public IReadOnlyList<ILogHandler> Handlers => _handlers;
        public InMemoryLogHandler MemoryHandler => _memoryHandler;

        public void Init(LogConfig config = null)
        {
            if (_initialized) return;

            _config = config;
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<LogConfig>();
            }

            _handlers.Clear();
            _memoryHandler = null;

            if (_config.EnableConsole)
            {
                _handlers.Add(new ConsoleLogHandler());
            }

            if (_config.EnableFile)
            {
                _handlers.Add(new FileLogHandler(_config));
            }

            if (_config.EnableMemory)
            {
                _memoryHandler = new InMemoryLogHandler(_config.MaxMemoryEntries);
                _handlers.Add(_memoryHandler);
            }

            _initialized = true;
            Info("LogManager initialized", LogCategory.Core, nameof(LogManager));
        }

        public void Shutdown()
        {
            for (int i = 0; i < _handlers.Count; i++)
            {
                try
                {
                    _handlers[i].Flush();
                    _handlers[i].Dispose();
                }
                catch (Exception e)
                {
	                // Log($"[Mutant][LogManager] Handler dispose failed: {e}");
                }
            }

            _handlers.Clear();
            _memoryHandler = null;
            _initialized = false;
        }

        public void AddHandler(ILogHandler handler)
        {
            if (handler == null) return;
            if (!_handlers.Contains(handler))
            {
                _handlers.Add(handler);
            }
        }

        public void RemoveHandler(ILogHandler handler)
        {
            if (handler == null) return;
            if (_handlers.Remove(handler))
            {
                handler.Dispose();
            }

            if (handler == _memoryHandler)
            {
                _memoryHandler = null;
            }
        }

        public IReadOnlyList<LogEntry> GetMemoryEntries()
        {
            EnsureInitialized();
            if (_memoryHandler == null)
                return Array.Empty<LogEntry>();

            return _memoryHandler.GetEntriesSnapshot();
        }

        public void ClearMemory()
        {
            EnsureInitialized();
            _memoryHandler?.Clear();
        }

        public void SetMinimumLevel(LogLevel level)
        {
            EnsureInitialized();
            _config.MinimumLevel = level;
        }

        public void SetEnableLog(bool enable)
        {
            EnsureInitialized();
            _config.EnableLog = enable;
        }

        public void Trace(string message, string category = null, string source = null)
            => Write(LogLevel.Trace, message, category, source);

        public void Debug(string message, string category = null, string source = null)
            => Write(LogLevel.Debug, message, category, source);

        public void Info(string message, string category = null, string source = null)
            => Write(LogLevel.Info, message, category, source);

        public void Warning(string message, string category = null, string source = null)
            => Write(LogLevel.Warning, message, category, source);

        public void Error(string message, string category = null, string source = null)
            => Write(LogLevel.Error, message, category, source);

        public void Fatal(string message, string category = null, string source = null)
            => Write(LogLevel.Fatal, message, category, source, includeStackTrace: true);

        public void Log(LogEntry entry)
        {
            EnsureInitialized();

            if (!_config.EnableLog) return;
            if (entry.Level < _config.MinimumLevel) return;

            for (int i = 0; i < _handlers.Count; i++)
            {
                try
                {
                    _handlers[i].Handle(entry);
                }
                catch (Exception e)
                {
                    //Debug.LogError($"[Mutant][LogManager] Handler error: {e}");
                }
            }
        }

        private void Write(LogLevel level, string message, string category, string source, bool includeStackTrace = false)
        {
            EnsureInitialized();

            string stackTrace = null;
            if (includeStackTrace || (_config != null && _config.IncludeStackTrace && level >= LogLevel.Error))
            {
                stackTrace = Environment.StackTrace;
            }

            int frame = (_config != null && _config.IncludeFrameCount) ? Time.frameCount : -1;

            var entry = new LogEntry(
                level,
                category,
                message,
                source,
                stackTrace,
                frame
            );

            Log(entry);
        }

        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                Init();
            }
        }
    }
}
