using System;

namespace Racso.EchoLogger
{
    public class EchoSystemLogger
    {
        private readonly LoggerCore loggerCore;
        private readonly string system;

        internal EchoSystemLogger(LoggerCore loggerCore, string system)
        {
            this.loggerCore = loggerCore ?? throw new ArgumentNullException(nameof(loggerCore));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        #region Debug Methods

        public void Debug(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Always, system, message);

        public void Debug<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Always, system, format, param1);

        public void Debug<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Always, system, format, param1, param2);

        public void Debug<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Always, system, format, param1, param2, param3);

        public void Debug<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Always, system, format, param1, param2, param3, param4);

        public void Debug1(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Once, system, message);

        public void Debug1<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Once, system, format, param1);

        public void Debug1<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Once, system, format, param1, param2);

        public void Debug1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Once, system, format, param1, param2, param3);

        public void Debug1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Debug, LogMode.Once, system, format, param1, param2, param3, param4);

        #endregion

        #region Info Methods

        public void Info(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Always, system, message);

        public void Info<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Always, system, format, param1);

        public void Info<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Always, system, format, param1, param2);

        public void Info<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Always, system, format, param1, param2, param3);

        public void Info<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Always, system, format, param1, param2, param3, param4);

        public void Info1(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Once, system, message);

        public void Info1<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Once, system, format, param1);

        public void Info1<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Once, system, format, param1, param2);

        public void Info1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Once, system, format, param1, param2, param3);

        public void Info1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Info, LogMode.Once, system, format, param1, param2, param3, param4);

        #endregion

        #region Warn Methods

        public void Warn(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Always, system, message);

        public void Warn<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Always, system, format, param1);

        public void Warn<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Always, system, format, param1, param2);

        public void Warn<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Always, system, format, param1, param2, param3);

        public void Warn<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Always, system, format, param1, param2, param3, param4);

        public void Warn1(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Once, system, message);

        public void Warn1<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Once, system, format, param1);

        public void Warn1<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Once, system, format, param1, param2);

        public void Warn1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Once, system, format, param1, param2, param3);

        public void Warn1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Warn, LogMode.Once, system, format, param1, param2, param3, param4);

        #endregion

        #region Error Methods

        public void Error(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Always, system, message);

        public void Error<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Always, system, format, param1);

        public void Error<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Always, system, format, param1, param2);

        public void Error<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Always, system, format, param1, param2, param3);

        public void Error<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Always, system, format, param1, param2, param3, param4);

        public void Error1(string message)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Once, system, message);

        public void Error1<T1>(string format, T1 param1)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Once, system, format, param1);

        public void Error1<T1, T2>(string format, T1 param1, T2 param2)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Once, system, format, param1, param2);

        public void Error1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Once, system, format, param1, param2, param3);

        public void Error1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
            => loggerCore.WriteIfEnabled(LogLevel.Error, LogMode.Once, system, format, param1, param2, param3, param4);

        #endregion
    }
}