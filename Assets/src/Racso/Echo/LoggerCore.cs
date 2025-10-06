using System;

namespace Racso.Echo
{
    internal class LoggerCore
    {
        private readonly LogWriter logWriter;
        private readonly EchoLogLevelsConfig echoLogLevelsConfig;
        private readonly HashesManager hashes;

        public LoggerCore(EchoLogLevelsConfig config, HashesManager hashes, LogWriter logger)
        {
            this.echoLogLevelsConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.hashes = hashes ?? throw new ArgumentNullException(nameof(hashes));
            this.logWriter = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private bool IsEnabled(string system, LogLevel level)
            => level <= echoLogLevelsConfig.GetSystemLevel(system);

        private bool ShouldLogOnce(string system, string message)
            => hashes.TryAdd(system, message);

        public void ClearHashes()
            => hashes.Clear();

        private void Write(LogLevel level, bool logOnce, string system, string message)
        {
            if (!logOnce || ShouldLogOnce(system, message))
                logWriter.WriteLog(level, system, message);
        }

        public void WriteIfEnabled(LogLevel level, bool logOnce, string system, string message)
        {
            if (!IsEnabled(system, level))
                Write(level, logOnce, system, message);
        }

        public void WriteIfEnabled<T1>(LogLevel level, bool logOnce, string system, string format, T1 param1)
        {
            if (IsEnabled(system, level))
                Write(level, logOnce, system, string.Format(format, param1));
        }

        public void WriteIfEnabled<T1, T2>(LogLevel level, bool logOnce, string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, level))
                Write(level, logOnce, system, string.Format(format, param1, param2));
        }

        public void WriteIfEnabled<T1, T2, T3>(LogLevel level, bool logOnce, string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, level))
                Write(level, logOnce, system, string.Format(format, param1, param2, param3));
        }

        public void WriteIfEnabled<T1, T2, T3, T4>(LogLevel level, bool logOnce, string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, level))
                Write(level, logOnce, system, string.Format(format, param1, param2, param3, param4));
        }
    }
}