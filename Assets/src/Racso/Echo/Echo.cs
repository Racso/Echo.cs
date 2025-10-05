using System;
using System.Collections.Generic;
using Racso.Echo.LogWriters;

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

    // Static convenience layer
    public static class CLogger
    {
        public static readonly EchoLogLevelsConfig Config = new();
        private static readonly LogWriter _logger = new UnityLogWriter(default);
        private static LoggerCore _instance = new(Config, _logger);

        // Debug methods
        public static void Debug(string system, string message) => _instance.Debug(system, message);
        public static void Debug<T1>(string system, string format, T1 param1) => _instance.Debug(system, format, param1);
        public static void Debug<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Debug(system, format, param1, param2);
        public static void Debug<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Debug(system, format, param1, param2, param3);
        public static void Debug<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Debug(system, format, param1, param2, param3, param4);

        public static void Debug1(string system, string message) => _instance.Debug1(system, message);
        public static void Debug1<T1>(string system, string format, T1 param1) => _instance.Debug1(system, format, param1);
        public static void Debug1<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Debug1(system, format, param1, param2);
        public static void Debug1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Debug1(system, format, param1, param2, param3);
        public static void Debug1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Debug1(system, format, param1, param2, param3, param4);

        // Info methods
        public static void Info(string system, string message) => _instance.Info(system, message);
        public static void Info<T1>(string system, string format, T1 param1) => _instance.Info(system, format, param1);
        public static void Info<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Info(system, format, param1, param2);
        public static void Info<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Info(system, format, param1, param2, param3);
        public static void Info<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Info(system, format, param1, param2, param3, param4);

        public static void Info1(string system, string message) => _instance.Info1(system, message);
        public static void Info1<T1>(string system, string format, T1 param1) => _instance.Info1(system, format, param1);
        public static void Info1<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Info1(system, format, param1, param2);
        public static void Info1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Info1(system, format, param1, param2, param3);
        public static void Info1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Info1(system, format, param1, param2, param3, param4);

        // Warn methods
        public static void Warn(string system, string message) => _instance.Warn(system, message);
        public static void Warn<T1>(string system, string format, T1 param1) => _instance.Warn(system, format, param1);
        public static void Warn<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Warn(system, format, param1, param2);
        public static void Warn<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Warn(system, format, param1, param2, param3);
        public static void Warn<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Warn(system, format, param1, param2, param3, param4);

        public static void Warn1(string system, string message) => _instance.Warn1(system, message);
        public static void Warn1<T1>(string system, string format, T1 param1) => _instance.Warn1(system, format, param1);
        public static void Warn1<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Warn1(system, format, param1, param2);
        public static void Warn1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Warn1(system, format, param1, param2, param3);
        public static void Warn1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Warn1(system, format, param1, param2, param3, param4);

        // Error methods
        public static void Error(string system, string message) => _instance.Error(system, message);
        public static void Error<T1>(string system, string format, T1 param1) => _instance.Error(system, format, param1);
        public static void Error<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Error(system, format, param1, param2);
        public static void Error<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Error(system, format, param1, param2, param3);
        public static void Error<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Error(system, format, param1, param2, param3, param4);
        public static void Error1(string system, string message) => _instance.Error1(system, message);
        public static void Error1<T1>(string system, string format, T1 param1) => _instance.Error1(system, format, param1);
        public static void Error1<T1, T2>(string system, string format, T1 param1, T2 param2) => _instance.Error1(system, format, param1, param2);
        public static void Error1<T1, T2, T3>(string system, string format, T1 param1, T2 param2, T3 param3) => _instance.Error1(system, format, param1, param2, param3);
        public static void Error1<T1, T2, T3, T4>(string system, string format, T1 param1, T2 param2, T3 param3, T4 param4) => _instance.Error1(system, format, param1, param2, param3, param4);
    }

    public struct SystemLogger
    {
        private readonly LoggerCore loggerCore;
        private readonly string system;

        public SystemLogger(LoggerCore loggerCore, string system)
        {
            this.loggerCore = loggerCore ?? throw new ArgumentNullException(nameof(loggerCore));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public void Debug(string message) => loggerCore.Debug(system, message);
        public void Debug<T1>(string format, T1 param1) => loggerCore.Debug(system, format, param1);
        public void Debug<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Debug(system, format, param1, param2);
        public void Debug<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Debug(system, format, param1, param2, param3);
        public void Debug<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Debug(system, format, param1, param2, param3, param4);
        public void Debug1(string message) => loggerCore.Debug1(system, message);
        public void Debug1<T1>(string format, T1 param1) => loggerCore.Debug1(system, format, param1);
        public void Debug1<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Debug1(system, format, param1, param2);
        public void Debug1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Debug1(system, format, param1, param2, param3);
        public void Debug1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Debug1(system, format, param1, param2, param3, param4);
        public void Info(string message) => loggerCore.Info(system, message);
        public void Info<T1>(string format, T1 param1) => loggerCore.Info(system, format, param1);
        public void Info<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Info(system, format, param1, param2);
        public void Info<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Info(system, format, param1, param2, param3);
        public void Info<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Info(system, format, param1, param2, param3, param4);
        public void Info1(string message) => loggerCore.Info1(system, message);
        public void Info1<T1>(string format, T1 param1) => loggerCore.Info1(system, format, param1);
        public void Info1<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Info1(system, format, param1, param2);
        public void Info1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Info1(system, format, param1, param2, param3);
        public void Info1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Info1(system, format, param1, param2, param3, param4);
        public void Warn(string message) => loggerCore.Warn(system, message);
        public void Warn<T1>(string format, T1 param1) => loggerCore.Warn(system, format, param1);
        public void Warn<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Warn(system, format, param1, param2);
        public void Warn<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Warn(system, format, param1, param2, param3);
        public void Warn<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Warn(system, format, param1, param2, param3, param4);
        public void Warn1(string message) => loggerCore.Warn1(system, message);
        public void Warn1<T1>(string format, T1 param1) => loggerCore.Warn1(system, format, param1);
        public void Warn1<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Warn1(system, format, param1, param2);
        public void Warn1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Warn1(system, format, param1, param2, param3);
        public void Warn1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Warn1(system, format, param1, param2, param3, param4);
        public void Error(string message) => loggerCore.Error(system, message);
        public void Error<T1>(string format, T1 param1) => loggerCore.Error(system, format, param1);
        public void Error<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Error(system, format, param1, param2);
        public void Error<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Error(system, format, param1, param2, param3);
        public void Error<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Error(system, format, param1, param2, param3, param4);
        public void Error1(string message) => loggerCore.Error1(system, message);
        public void Error1<T1>(string format, T1 param1) => loggerCore.Error1(system, format, param1);
        public void Error1<T1, T2>(string format, T1 param1, T2 param2) => loggerCore.Error1(system, format, param1, param2);
        public void Error1<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3) => loggerCore.Error1(system, format, param1, param2, param3);
        public void Error1<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4) => loggerCore.Error1(system, format, param1, param2, param3, param4);
    }
}