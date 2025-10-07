using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

#if UNITY_EDITOR

namespace Racso.Echo.Editor
{
    public static class EchoEditor
    {
        public static void SetSystems(Type type)
        {
            EchoEditorState.SetSystemNames(type);
        }
    }

    internal static class EchoEditorState
    {
        private static EchoLogLevelsConfig _logLevels;
        private static List<string> _systemNames = new();

        public static EchoLogLevelsConfig LogLevels
        {
            get => _logLevels;

            set
            {
                Clear();
                _logLevels = value;
                if (_logLevels != null)
                {
                    InitializeFromEditorPrefs();
                    _logLevels.Updated += OnLevelsUpdated;
                }
            }
        }

        public static List<string> SystemNames => _systemNames;

        public static void SetSystemNames(Type type)
        {
            _systemNames = GetStaticStringMembers(type);
        }

        public static List<string> GetStaticStringMembers(Type staticClassType)
        {
            List<string> result = new List<string>();

            foreach (FieldInfo field in staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(string))
                {
                    string value = field.GetValue(null) as string;
                    if (value != null)
                        result.Add(value);
                }
            }

            // Get public static properties
            foreach (PropertyInfo prop in staticClassType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (prop.PropertyType == typeof(string) && prop.CanRead)
                {
                    string value = prop.GetValue(null) as string;
                    if (value != null)
                        result.Add(value);
                }
            }

            return result;
        }

        private static void InitializeFromEditorPrefs()
        {
            if (_logLevels == null)
                return;

            string key = "Racso.Echo";
            string json = UnityEditor.EditorPrefs.GetString(key, null);
            if (string.IsNullOrEmpty(json))
                return;

            EditorPrefsData data = JsonConvert.DeserializeObject<EditorPrefsData>(json);
            if (data == null)
                return;

            _logLevels.SetDefaultLevel(data.DefaultLevel);
            _logLevels.ClearSystemLevels();
            foreach (KeyValuePair<string, LogLevel> kvp in data.SystemLevels)
                _logLevels.SetSystemLevel(kvp.Key, kvp.Value);
        }

        private static void OnLevelsUpdated()
        {
            if (_logLevels == null)
                return;

            EditorPrefsData data = new()
            {
                DefaultLevel = _logLevels.DefaultLevel,
                SystemLevels = new Dictionary<string, LogLevel>()
            };

            foreach (KeyValuePair<string, LogLevel> field in _logLevels.GetAllSystemLevels())
                data.SystemLevels[field.Key] = field.Value;

            string json = JsonConvert.SerializeObject(data);
            string key = "Racso.Echo";
            UnityEditor.EditorPrefs.SetString(key, json);

            Debug.Log("Echo log levels updated and saved to EditorPrefs: " + json);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Clear()
        {
            if (_logLevels != null)
                _logLevels.Updated -= OnLevelsUpdated;
            _logLevels = null;
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