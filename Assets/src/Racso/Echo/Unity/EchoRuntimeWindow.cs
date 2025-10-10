using UnityEngine;

namespace Racso.Echo.Unity
{
    public class EchoRuntimeWindow : MonoBehaviour
    {
        public static bool Visible { get; set; } = false;
        private Rect windowRect = new Rect(100, 100, 400, 400);
        private const string PrefsKey = "Echo.Config";

        private void OnEnable()
        {
            EchoEditor.Updated += OnEchoEditorUpdated;
            LoadSettingsFromPlayerPrefs();
        }

        private void OnDisable()
        {
            EchoEditor.Updated -= OnEchoEditorUpdated;
        }

        private void OnEchoEditorUpdated()
        {
            SaveSettingsToPlayerPrefs();
        }

        private void SaveSettingsToPlayerPrefs()
        {
            EchoSettings settings = EchoEditor.Settings;
            if (settings != null)
            {
                string json = JsonUtility.ToJson(settings);
                PlayerPrefs.SetString(PrefsKey, json);
                PlayerPrefs.Save();
            }
        }

        private void LoadSettingsFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(PrefsKey))
            {
                string json = PlayerPrefs.GetString(PrefsKey);
                EchoSettings settings = EchoEditor.Settings;
                if (settings != null)
                {
                    JsonUtility.FromJsonOverwrite(json, settings);
                }
            }
        }


        void OnGUI()
        {
            if (!Visible) return;
            windowRect = GUI.Window(12345, windowRect, DrawWindow, "Echo Log Levels");
        }

        void DrawWindow(int windowID)
        {
            EchoSettings settings = EchoEditor.Settings;
            var systemNames = EchoEditor.SystemNames;

            if (settings == null)
            {
                GUILayout.Label("No EchoLogLevelsConfig assigned.");
                GUI.DragWindow();
                return;
            }

            GUILayout.Label("Default Log Level", GUI.skin.label);
            LogLevel defaultLevel = settings.DefaultLevel;
            LogLevel newDefault = (LogLevel)GUILayout.SelectionGrid((int)defaultLevel, System.Enum.GetNames(typeof(LogLevel)), System.Enum.GetNames(typeof(LogLevel)).Length);
            if (newDefault != defaultLevel)
            {
                settings.SetDefaultLevel(newDefault);
            }

            GUILayout.Space(10);
            GUILayout.Label("System Log Levels", GUI.skin.label);

            string[] options = System.Enum.GetNames(typeof(LogLevel));
            string[] popupOptions = new string[options.Length + 1];
            popupOptions[0] = "Default";
            for (int i = 0; i < options.Length; i++)
                popupOptions[i + 1] = options[i];

            foreach (string systemName in systemNames)
            {
                LogLevel level;
                bool hasExplicit = settings.TryGetSystemLevel(systemName, out level);
                int selectedIndex = hasExplicit ? (int)level + 1 : 0;

                GUILayout.BeginHorizontal();
                GUILayout.Label(systemName, GUILayout.Width(120));
                int newIndex = GUILayout.SelectionGrid(selectedIndex, popupOptions, popupOptions.Length);

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
                    GUILayout.Label("(Default)", GUILayout.Width(70));
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Close"))
                Visible = false;

            GUI.DragWindow();
        }
    }
}