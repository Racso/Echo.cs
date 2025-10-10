#if UNITY_2017_1_OR_NEWER

using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.Racso.EchoLogger.LogWriters;
using UnityEngine;

namespace Racso.EchoLogger.Unity
{
    public static class EchoUnity
    {
        public static event Action Updated;

        public static void SetupWindows(Echo echo, IEnumerable<string> systemNames)
        {
            Clear();
            Settings = echo.Settings;
            if (Settings != null)
            {
                Settings.Updated += OnLevelsUpdated;
            }

            foreach (string systemName in systemNames)
                SystemNames.Add(systemName);
        }

        public static void SetupWindows(Echo echo, Type systemNamesClass)
            => SetupWindows(echo, GetStaticStringMembers(systemNamesClass));

        public static EchoSettings Settings { get; private set; }

        public static List<string> SystemNames { get; } = new();

        private static List<string> GetStaticStringMembers(Type staticClassType)
        {
            List<string> result = new List<string>();

            foreach (FieldInfo field in staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(string) && field.GetValue(null) is string value)
                    result.Add(value);
            }

            foreach (PropertyInfo prop in staticClassType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (prop.PropertyType == typeof(string) && prop.CanRead && prop.GetValue(null) is string value)
                    result.Add(value);
            }

            return result;
        }

        private static void OnLevelsUpdated()
        {
            Updated?.Invoke();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear()
        {
            if (Settings != null)
                Settings.Updated -= OnLevelsUpdated;
            Settings = null;
            SystemNames.Clear();
            Updated = null;
        }

        public static Echo NewDefaultEcho(LogWriterConfig config = null)
        {
            config ??= new LogWriterConfig
            {
                SystemColor = SystemColor.LabelOnly,
                LevelLabels = false, // Unity already has them
                Timestamp = false // Unity already has it
            };

            UnityLogWriter writer = new(config);
            return new Echo(writer);
        }
    }
}

#endif