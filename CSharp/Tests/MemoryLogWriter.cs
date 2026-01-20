using System.Collections.Generic;

namespace Racso.EchoLogger.Tests
{
    /// <summary>
    /// A simple in-memory log writer for testing purposes.
    /// Stores all log entries so they can be validated in tests.
    /// </summary>
    public class MemoryLogWriter : EchoLogWriter
    {
        public class LogEntry
        {
            public LogLevel Level { get; set; }
            public string System { get; set; }
            public string Message { get; set; }

            public LogEntry(LogLevel level, string system, string message)
            {
                Level = level;
                System = system;
                Message = message;
            }

            public override string ToString()
            {
                return $"[{Level}] [{System}] {Message}";
            }
        }

        private readonly List<LogEntry> logs = new List<LogEntry>();

        public void WriteLog(LogLevel level, string system, string message)
        {
            logs.Add(new LogEntry(level, system, message));
        }

        public IReadOnlyList<LogEntry> GetLogs() => logs;

        public void Clear() => logs.Clear();

        public int Count => logs.Count;
    }
}
