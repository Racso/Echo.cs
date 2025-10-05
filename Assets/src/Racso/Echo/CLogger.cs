using Racso.Echo.LogWriters;

namespace Racso.Echo
{
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
}