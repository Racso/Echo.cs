using System;
using System.Collections.Generic;

namespace Racso.Echo
{
    public class LoggerCore
    {
        private readonly LogWriter logWriter;
        private readonly EchoLogLevelsConfig echoLogLevelsConfig;
        private readonly HashSet<uint> onceHashes = new();

        public LoggerCore(EchoLogLevelsConfig config, LogWriter logger)
        {
            this.echoLogLevelsConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.logWriter = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void ClearOnceHashes()
        {
            onceHashes.Clear();
        }

        private bool IsEnabled(string system, LogLevel level)
        {
            return level <= echoLogLevelsConfig.GetSystemLevel(system);
        }

        private bool ShouldLogOnce(string system, string message)
        {
            uint hash = CoreLogHelper.FNV1a32(message);
            hash = CoreLogHelper.FNV1a32(system, hash);
            return onceHashes.Add(hash);
        }

        private void WriteLog(LogLevel level, string system, string message)
        {
            logWriter.WriteLog(level, system, message);
        }

        private void TryLogOnce(string system, LogLevel level, string message)
        {
            if (ShouldLogOnce(system, message))
                WriteLog(level, system, message);
        }

        #region Debug Methods

        public void Debug(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Debug))
                WriteLog(LogLevel.Debug, system, message);
        }

        public void Debug<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Debug))
                WriteLog(LogLevel.Debug, system, string.Format(format, param1));
        }

        public void Debug<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Debug))
                WriteLog(LogLevel.Debug, system, string.Format(format, param1, param2));
        }

        public void Debug<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Debug))
                WriteLog(LogLevel.Debug, system, string.Format(format, param1, param2, param3));
        }

        public void Debug<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Debug))
                WriteLog(LogLevel.Debug, system, string.Format(format, param1, param2, param3, param4));
        }

        public void Debug1(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Debug))
                TryLogOnce(system, LogLevel.Debug, message);
        }

        public void Debug1<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Debug))
                TryLogOnce(system, LogLevel.Debug, string.Format(format, param1));
        }

        public void Debug1<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Debug))
                TryLogOnce(system, LogLevel.Debug, string.Format(format, param1, param2));
        }

        public void Debug1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Debug))
                TryLogOnce(system, LogLevel.Debug, string.Format(format, param1, param2, param3));
        }

        public void Debug1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Debug))
                TryLogOnce(system, LogLevel.Debug, string.Format(format, param1, param2, param3, param4));
        }

        #endregion

        #region Info Methods

        public void Info(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Info))
                WriteLog(LogLevel.Info, system, message);
        }

        public void Info<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Info))
                WriteLog(LogLevel.Info, system, string.Format(format, param1));
        }

        public void Info<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Info))
                WriteLog(LogLevel.Info, system, string.Format(format, param1, param2));
        }

        public void Info<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Info))
                WriteLog(LogLevel.Info, system, string.Format(format, param1, param2, param3));
        }

        public void Info<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Info))
                WriteLog(LogLevel.Info, system, string.Format(format, param1, param2, param3, param4));
        }

        public void Info1(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Info))
                TryLogOnce(system, LogLevel.Info, message);
        }

        public void Info1<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Info))
                TryLogOnce(system, LogLevel.Info, string.Format(format, param1));
        }

        public void Info1<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Info))
                TryLogOnce(system, LogLevel.Info, string.Format(format, param1, param2));
        }

        public void Info1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Info))
                TryLogOnce(system, LogLevel.Info, string.Format(format, param1, param2, param3));
        }

        public void Info1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Info))
                TryLogOnce(system, LogLevel.Info, string.Format(format, param1, param2, param3, param4));
        }

        #endregion

        #region Warn Methods

        public void Warn(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Warn))
                WriteLog(LogLevel.Warn, system, message);
        }

        public void Warn<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Warn))
                WriteLog(LogLevel.Warn, system, string.Format(format, param1));
        }

        public void Warn<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Warn))
                WriteLog(LogLevel.Warn, system, string.Format(format, param1, param2));
        }

        public void Warn<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Warn))
                WriteLog(LogLevel.Warn, system, string.Format(format, param1, param2, param3));
        }

        public void Warn<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Warn))
                WriteLog(LogLevel.Warn, system, string.Format(format, param1, param2, param3, param4));
        }

        public void Warn1(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Warn))
                TryLogOnce(system, LogLevel.Warn, message);
        }

        public void Warn1<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Warn))
                TryLogOnce(system, LogLevel.Warn, string.Format(format, param1));
        }

        public void Warn1<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Warn))
                TryLogOnce(system, LogLevel.Warn, string.Format(format, param1, param2));
        }

        public void Warn1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Warn))
                TryLogOnce(system, LogLevel.Warn, string.Format(format, param1, param2, param3));
        }

        public void Warn1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Warn))
                TryLogOnce(system, LogLevel.Warn, string.Format(format, param1, param2, param3, param4));
        }

        #endregion

        #region Error Methods

        public void Error(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Error))
                WriteLog(LogLevel.Error, system, message);
        }

        public void Error<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Error))
                WriteLog(LogLevel.Error, system, string.Format(format, param1));
        }

        public void Error<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Error))
                WriteLog(LogLevel.Error, system, string.Format(format, param1, param2));
        }

        public void Error<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Error))
                WriteLog(LogLevel.Error, system, string.Format(format, param1, param2, param3));
        }

        public void Error<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Error))
                WriteLog(LogLevel.Error, system, string.Format(format, param1, param2, param3, param4));
        }

        public void Error1(string system, string message)
        {
            if (IsEnabled(system, LogLevel.Error))
                TryLogOnce(system, LogLevel.Error, message);
        }

        public void Error1<T1>(string system, string format, T1 param1)
        {
            if (IsEnabled(system, LogLevel.Error))
                TryLogOnce(system, LogLevel.Error, string.Format(format, param1));
        }

        public void Error1<T1, T2>(string system, string format, T1 param1, T2 param2)
        {
            if (IsEnabled(system, LogLevel.Error))
                TryLogOnce(system, LogLevel.Error, string.Format(format, param1, param2));
        }

        public void Error1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(system, LogLevel.Error))
                TryLogOnce(system, LogLevel.Error, string.Format(format, param1, param2, param3));
        }

        public void Error1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (IsEnabled(system, LogLevel.Error))
                TryLogOnce(system, LogLevel.Error, string.Format(format, param1, param2, param3, param4));
        }

        #endregion
    }
}