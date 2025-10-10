using UnityEngine;

namespace Racso.EchoLogger.Unity
{
    public class EchoRuntimeWindow : MonoBehaviour
    {
        public bool Visible { get; set; } = false;
        private Rect windowRect = new Rect(0, 0, 600, 400);
        private Vector2 scrollPos = Vector2.zero;
        private const string PrefsKey = "Echo.Config";

        private void OnEnable()
        {
            EchoUnity.Updated += OnEchoEditorUpdated;
            LoadSettingsFromPlayerPrefs();
        }

        private void OnDisable()
        {
            EchoUnity.Updated -= OnEchoEditorUpdated;
        }

        private void OnEchoEditorUpdated()
        {
            SaveSettingsToPlayerPrefs();
        }

        private void SaveSettingsToPlayerPrefs()
        {
            EchoSettings settings = EchoUnity.Settings;
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
                EchoSettings settings = EchoUnity.Settings;
                if (settings != null)
                {
                    JsonUtility.FromJsonOverwrite(json, settings);
                }
            }
        }


        void OnGUI()
        {
            if (!Visible) return;
            // Dynamically scale window size based on screen resolution
            float w = Mathf.Clamp(Screen.width * 0.9f, 480, 900);
            float h = Mathf.Clamp(Screen.height * 0.9f, 240, 700);
            windowRect.width = w;
            windowRect.height = h;
            windowRect.x = 0;
            windowRect.y = 0;
            windowRect = GUI.Window(12345, windowRect, DrawWindow, "Echo Log Levels");
        }

        void DrawWindow(int windowID)
        {
            EchoSettings settings = EchoUnity.Settings;
            var systemNames = EchoUnity.SystemNames;

            if (settings == null)
            {
                GUILayout.Label("No EchoLogLevelsConfig assigned.");
                GUI.DragWindow();
                return;
            }

            GUILayout.Label("Default Log Level", GUI.skin.label);
            LogLevel defaultLevel = settings.DefaultLevel;
            LogLevel newDefault = (LogLevel)GUILayout.SelectionGrid((int)defaultLevel, System.Enum.GetNames(typeof(LogLevel)), System.Enum.GetNames(typeof(LogLevel)).Length, GUILayout.Width(windowRect.width - 40));
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

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false); //, GUILayout.Height(windowRect.height - 120));
            foreach (string systemName in systemNames)
            {
                LogLevel level;
                bool hasExplicit = settings.TryGetSystemLevel(systemName, out level);
                int selectedIndex = hasExplicit ? (int)level + 1 : 0;

                GUILayout.BeginVertical("box");
                GUILayout.Label(systemName, GUILayout.Width(windowRect.width - 120));
                GUILayout.BeginHorizontal();
                float buttonWidth = (windowRect.width - 120) / popupOptions.Length;
                for (int i = 0; i < popupOptions.Length; i++)
                {
                    bool selected = (selectedIndex == i);
                    if (GUILayout.Toggle(selected, popupOptions[i], "Button", GUILayout.Width(buttonWidth)))
                    {
                        if (i == 0 && hasExplicit)
                        {
                            settings.ClearSystemLevel(systemName);
                        }
                        else if (i > 0)
                        {
                            LogLevel selectedLevel = (LogLevel)(i - 1);
                            if (!hasExplicit || selectedLevel != level)
                                settings.SetSystemLevel(systemName, selectedLevel);
                        }

                        selectedIndex = i;
                    }
                }

                if (!hasExplicit)
                {
                    GUILayout.Label("(Default)", GUILayout.Width(70));
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();

            if (GUILayout.Button("Close"))
                Visible = false;

            GUI.DragWindow();
        }
    }
}