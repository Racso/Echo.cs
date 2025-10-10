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
        public EchoFactory(LogWriterConfig config = null) : this(GetUnityLogger(config))
        {
        }

        private static UnityLogWriter GetUnityLogger(LogWriterConfig config)
        {
            config ??= new LogWriterConfig
            {
                SystemColor = SystemColor.LabelOnly,
                LevelLabels = false, // Unity already has them
                Timestamp = false // Unity already has it
            };

            return new UnityLogWriter(config);
        }
#else
        public EchoFactory(LogWriterConfig config = null) : this(GetConsoleLogger(config))
        {
        }

        private static ConsoleLogWriter GetConsoleLogger(LogWriterConfig config)
        {
            config ??= new LogWriterConfig();
            return new ConsoleLogWriter(config);
        }
#endif

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