using System;
using System.Collections.Generic;

namespace Racso.Echo
{
    public class EchoSettings
    {
        private readonly Dictionary<string, LogLevel> systemLevels = new();

        public LogLevel DefaultLevel { get; private set; } = LogLevel.Warn;
        public event Action Updated;

        public void SetSystemLevel(string system, LogLevel level)
        {
            ThrowIfInvalidSystem(system);
            systemLevels[system] = level;
            Updated?.Invoke();
        }

        public void ClearSystemLevel(string system)
        {
            ThrowIfInvalidSystem(system);
            if (systemLevels.Remove(system))
                Updated?.Invoke();
        }

        public LogLevel GetSystemLevel(string system)
        {
            ThrowIfInvalidSystem(system);
            return systemLevels.GetValueOrDefault(system, DefaultLevel);
        }

        public bool TryGetSystemLevel(string system, out LogLevel level)
        {
            ThrowIfInvalidSystem(system);
            return systemLevels.TryGetValue(system, out level);
        }

        public void ClearSystemLevels()
        {
            systemLevels.Clear();
            Updated?.Invoke();
        }

        public void SetDefaultLevel(LogLevel level)
        {
            DefaultLevel = level;
            Updated?.Invoke();
        }

        public IReadOnlyDictionary<string, LogLevel> GetAllSystemLevels() => systemLevels;

        private void ThrowIfInvalidSystem(string system)
        {
            if (string.IsNullOrEmpty(system))
                throw new ArgumentException("System name cannot be null or empty.", nameof(system));
        }
    }
}