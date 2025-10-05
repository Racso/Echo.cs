using System;

namespace Racso.Echo
{
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