#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;

namespace Racso.Echo.Editor
{
    public class EchoLogLevelEditorWindow : EditorWindow
    {
        private EchoSettings settings;
        private List<string> systemNames;
        private LogLevel defaultLevel;
        private readonly Dictionary<string, LogLevel> systemLevels = new();

        [MenuItem("Tools/Racso/Echo Log Level Config")]
        public static void ShowWindow()
        {
            GetWindow<EchoLogLevelEditorWindow>("Echo Log Levels");
        }

        private void TryInitialize()
        {
            if (settings != null)
                return;

            settings = EchoEditor.Settings;
            systemNames = EchoEditor.SystemNames;
            if (settings == null)
                return;

            defaultLevel = settings.DefaultLevel;
            systemLevels.Clear();
            foreach (string systemName in systemNames)
            {
                systemLevels[systemName] = settings.GetSystemLevel(systemName);
            }
        }

        private void OnGUI()
        {
            TryInitialize();

            if (settings == null)
            {
                EditorGUILayout.HelpBox("No EchoLogLevelsConfig assigned.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Set All Log Level", EditorStyles.boldLabel);
            LogLevel newDefault = (LogLevel)EditorGUILayout.EnumPopup(defaultLevel);
            if (newDefault != defaultLevel)
            {
                defaultLevel = newDefault;
                settings.SetDefaultLevel(defaultLevel);
                settings.ClearSystemLevels();
                // Update UI to reflect cleared system levels
                foreach (string systemName in systemNames)
                {
                    systemLevels[systemName] = defaultLevel;
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("System Log Levels", EditorStyles.boldLabel);

            foreach (string name in systemNames)
            {
                LogLevel current = systemLevels[name];
                LogLevel newLevel = (LogLevel)EditorGUILayout.EnumPopup(name, current);
                if (newLevel != current)
                {
                    systemLevels[name] = newLevel;
                    settings.SetSystemLevel(name, newLevel);
                }
            }
        }
    }
}
#endif