#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Racso.Echo.Editor
{
    public static class EchoEditor
    {
        private const string EditorPrefsKey = "Racso.Echo";

        public static void Setup(EchoSettings settings, IEnumerable<string> systemNames)
        {
            Clear();
            Settings = settings;
            if (Settings != null)
            {
                InitializeFromEditorPrefs();
                Settings.Updated += OnLevelsUpdated;
            }

            foreach (string systemName in systemNames)
                SystemNames.Add(systemName);
        }

        public static void Setup(EchoSettings settings, Type systemNamesClass)
            => Setup(settings, GetStaticStringMembers(systemNamesClass));

        internal static EchoSettings Settings { get; private set; }

        internal static List<string> SystemNames { get; } = new();

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

        private static void InitializeFromEditorPrefs()
        {
            if (Settings == null)
                return;

            string json = UnityEditor.EditorPrefs.GetString(EditorPrefsKey, null);
            if (string.IsNullOrEmpty(json))
                return;

            EditorPrefsData data = JsonConvert.DeserializeObject<EditorPrefsData>(json);
            if (data == null)
                return;

            Settings.SetDefaultLevel(data.DefaultLevel);
            Settings.ClearSystemLevels();
            foreach (var kvp in data.SystemLevels)
                Settings.SetSystemLevel(kvp.Key, kvp.Value);
        }

        private static void OnLevelsUpdated()
        {
            if (Settings == null)
                return;

            EditorPrefsData data = new()
            {
                DefaultLevel = Settings.DefaultLevel,
                SystemLevels = new Dictionary<string, LogLevel>()
            };

            foreach (KeyValuePair<string, LogLevel> field in Settings.GetAllSystemLevels())
                data.SystemLevels[field.Key] = field.Value;

            string json = JsonConvert.SerializeObject(data);
            UnityEditor.EditorPrefs.SetString(EditorPrefsKey, json);

            Debug.Log("Echo log levels updated and saved to EditorPrefs: " + json);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear()
        {
            if (Settings != null)
                Settings.Updated -= OnLevelsUpdated;
            Settings = null;
            SystemNames.Clear();
        }

        [Serializable]
        private class EditorPrefsData
        {
            public LogLevel DefaultLevel;
            public Dictionary<string, LogLevel> SystemLevels = new();
        }
    }
}

#endif