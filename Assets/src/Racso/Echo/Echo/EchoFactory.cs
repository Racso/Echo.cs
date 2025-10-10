using System;
using System.Collections.Generic;
using Racso.Echo.LogWriters;

namespace Racso.Echo
{
    public class EchoFactory
    {
        private readonly LoggerCore loggerCore;
        private readonly Dictionary<string, object> loggers = new();
        private readonly EchoSettings levelsConfig;

        public EchoFactory(EchoLogWriter writer)
        {
            writer = writer ?? throw new ArgumentNullException(nameof(writer));
            HashesManager hashes = new();
            levelsConfig = new();
            loggerCore = new LoggerCore(levelsConfig, hashes, writer);
        }

#if UNITY_2017_1_OR_NEWER
        public EchoFactory(LogWriterConfig config = null) : this(new UnityLogWriter(GetOrDefaultConfig(config)))
        {
        }
#else
        public EchoFactory(LogWriterConfig config = null) : this(new ConsoleLogWriter(GetOrDefaultConfig(config)))
        {
        }
#endif

        private static LogWriterConfig GetOrDefaultConfig(LogWriterConfig config)
        {
            return config ?? new LogWriterConfig
            {
                SystemColors = true,
                Timestamp = true,
            };
        }

        public EchoLogger GetLogger()
        {
            if (!loggers.ContainsKey(""))
                loggers[""] = new EchoLogger(loggerCore);
            return (EchoLogger)loggers[""];
        }

        public EchoSystemLogger GetSystemLogger(string system)
        {
            if (string.IsNullOrEmpty(system))
                throw new ArgumentException("System name cannot be null or empty.", nameof(system));

            if (!loggers.ContainsKey(system))
                loggers[system] = new EchoSystemLogger(loggerCore, system);
            return (EchoSystemLogger)loggers[system];
        }

        public EchoSettings LogLevels => levelsConfig;
    }
}