#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;

namespace Racso.Echo.Editor
{
    public class EchoLogLevelEditorWindow : EditorWindow
    {
        private EchoLogLevelsConfig _config;
        private List<string> _systemNames;
        private LogLevel _defaultLevel;
        private Dictionary<string, LogLevel> _systemLevels = new();

        [MenuItem("Tools/Racso/Echo Log Level Config")]
        public static void ShowWindow()
        {
            GetWindow<EchoLogLevelEditorWindow>("Echo Log Levels");
        }

        private void TryInitialize()
        {
            if (_config != null)
                return;

            _config = EchoEditorState.LogLevels;
            _systemNames = EchoEditorState.SystemNames;
            if (_config == null)
                return;

            _defaultLevel = _config.DefaultLevel;
            _systemLevels.Clear();
            foreach (var name in _systemNames)
            {
                _systemLevels[name] = _config.GetSystemLevel(name);
            }
        }

        private void OnGUI()
        {
            TryInitialize();

            if (_config == null)
            {
                EditorGUILayout.HelpBox("No EchoLogLevelsConfig assigned.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Set All Log Level", EditorStyles.boldLabel);
            var newDefault = (LogLevel)EditorGUILayout.EnumPopup(_defaultLevel);
            if (newDefault != _defaultLevel)
            {
                _defaultLevel = newDefault;
                _config.SetDefaultLevel(_defaultLevel);
                _config.ClearSystemLevels();
                // Update UI to reflect cleared system levels
                foreach (var name in _systemNames)
                {
                    _systemLevels[name] = _defaultLevel;
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("System Log Levels", EditorStyles.boldLabel);

            foreach (var name in _systemNames)
            {
                var current = _systemLevels[name];
                var newLevel = (LogLevel)EditorGUILayout.EnumPopup(name, current);
                if (newLevel != current)
                {
                    _systemLevels[name] = newLevel;
                    _config.SetSystemLevel(name, newLevel);
                }
            }
        }
    }
}
#endif