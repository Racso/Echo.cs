#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Racso.Echo.Editor
{
    public class EchoLogLevelEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Racso/Echo Log Level Config")]
        public static void ShowWindow()
        {
            GetWindow<EchoLogLevelEditorWindow>("Echo Log Levels");
        }

        private void OnGUI()
        {
            EchoSettings settings = EchoEditor.Settings;
            List<string> systemNames = EchoEditor.SystemNames;

            if (settings == null)
            {
                EditorGUILayout.HelpBox("No EchoLogLevelsConfig assigned.", MessageType.Warning);
                return;
            }

            // Default Level Section
            EditorGUILayout.LabelField("Default Log Level", EditorStyles.boldLabel);
            LogLevel defaultLevel = settings.DefaultLevel;
            LogLevel newDefault = (LogLevel)EditorGUILayout.EnumPopup(defaultLevel);
            if (newDefault != defaultLevel)
            {
                settings.SetDefaultLevel(newDefault);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("System Log Levels", EditorStyles.boldLabel);

            string[] options = System.Enum.GetNames(typeof(LogLevel));
            string[] popupOptions = new string[options.Length + 1];
            popupOptions[0] = "Default";
            for (int i = 0; i < options.Length; i++)
                popupOptions[i + 1] = options[i];

            foreach (string systemName in systemNames)
            {
                LogLevel level;
                bool hasExplicit = settings.TryGetSystemLevel(systemName, out level);

                // Build popup options
                int selectedIndex = hasExplicit ? (int)level + 1 : 0;

                EditorGUILayout.BeginHorizontal();
                int newIndex = EditorGUILayout.Popup(systemName, selectedIndex, popupOptions);

                if (newIndex == 0 && hasExplicit)
                {
                    settings.ClearSystemLevel(systemName);
                }
                else if (newIndex > 0)
                {
                    LogLevel selectedLevel = (LogLevel)(newIndex - 1);
                    if (!hasExplicit || selectedLevel != level)
                        settings.SetSystemLevel(systemName, selectedLevel);
                }

                if (!hasExplicit)
                {
                    EditorGUILayout.LabelField("(Default)", GUILayout.Width(70));
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif