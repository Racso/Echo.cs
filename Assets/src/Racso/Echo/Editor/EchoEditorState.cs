using System.Collections.Generic;

#if UNITY_2017_1_OR_NEWER

namespace Racso.Echo.Editor
{
    internal static class EchoEditorState
    {
        private static EchoLogLevelsConfig _logLevels;

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

        private static void InitializeFromEditorPrefs()
        {
            if (_logLevels == null)
                return;

            string key = "Racso.Echo";
            string json = UnityEditor.EditorPrefs.GetString(key, null);
            if (string.IsNullOrEmpty(json))
                return;

            EditorPrefsData data = UnityEngine.JsonUtility.FromJson<EditorPrefsData>(json);
            if (data == null)
                return;

            _logLevels.SetDefaultLevel(data.DefaultLevel);
            _logLevels.ClearSystemLevels();
            foreach (var kvp in data.SystemLevels)
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

            // Assuming we have access to the internal dictionary of system levels.
            // If not, we would need to add a method in EchoLogLevelsConfig to get all system levels.
            foreach (var field in typeof(EchoLogLevelsConfig).GetField("systemLevels", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_logLevels) as Dictionary<string, LogLevel>)
            {
                data.SystemLevels[field.Key] = field.Value;
            }

            string json = UnityEngine.JsonUtility.ToJson(data);
            string key = "Racso.Echo";
            UnityEditor.EditorPrefs.SetString(key, json);
        }

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Clear()
        {
            if (LogLevels != null)
                LogLevels.Updated -= OnLevelsUpdated;
            LogLevels = null;
        }

        private class EditorPrefsData
        {
            public LogLevel DefaultLevel;
            public Dictionary<string, LogLevel> SystemLevels = new();
        }
    }

    // Write an Editor Window to configure the log levels on runtime.
}

#endif