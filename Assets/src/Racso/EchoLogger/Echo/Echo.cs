using System;
using System.Collections.Generic;

namespace Racso.EchoLogger
{
    public class Echo
    {
        private readonly LoggerCore loggerCore;
        private readonly Dictionary<string, object> loggers = new();
        private readonly EchoSettings settings;

        public Echo(EchoLogWriter writer)
        {
            writer = writer ?? throw new ArgumentNullException(nameof(writer));
            HashesManager hashes = new();
            settings = new();
            loggerCore = new LoggerCore(settings, hashes, writer);
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

        public EchoSettings Settings => settings;
    }
}