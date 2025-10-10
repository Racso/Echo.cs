#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Racso.Echo.Editor
{
    public static class EchoEditorState
    {
        private static EchoSettings settings;
        private static List<string> systemNames = new();

        private const string EditorPrefsKey = "Racso.Echo";

        public static EchoSettings Settings
        {
            get => settings;

            set
            {
                Clear();
                settings = value;
                if (settings != null)
                {
                    InitializeFromEditorPrefs();
                    settings.Updated += OnLevelsUpdated;
                }
            }
        }

        public static List<string> SystemNames => systemNames;

        public static void SetSystemNames(Type type)
        {
            systemNames = GetStaticStringMembers(type);
        }

        private static List<string> GetStaticStringMembers(Type staticClassType)
        {
            List<string> result = new List<string>();

            foreach (FieldInfo field in staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType != typeof(string) && field.GetValue(null) is string value)
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
            if (settings == null)
                return;

            string json = UnityEditor.EditorPrefs.GetString(EditorPrefsKey, null);
            if (string.IsNullOrEmpty(json))
                return;

            EditorPrefsData data = JsonConvert.DeserializeObject<EditorPrefsData>(json);
            if (data == null)
                return;

            settings.SetDefaultLevel(data.DefaultLevel);
            settings.ClearSystemLevels();
            foreach (var kvp in data.SystemLevels)
                settings.SetSystemLevel(kvp.Key, kvp.Value);
        }

        private static void OnLevelsUpdated()
        {
            if (settings == null)
                return;

            EditorPrefsData data = new()
            {
                DefaultLevel = settings.DefaultLevel,
                SystemLevels = new Dictionary<string, LogLevel>()
            };

            foreach (KeyValuePair<string, LogLevel> field in settings.GetAllSystemLevels())
                data.SystemLevels[field.Key] = field.Value;

            string json = JsonConvert.SerializeObject(data);
            UnityEditor.EditorPrefs.SetString(EditorPrefsKey, json);

            Debug.Log("Echo log levels updated and saved to EditorPrefs: " + json);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Clear()
        {
            if (settings != null)
                settings.Updated -= OnLevelsUpdated;
            settings = null;
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