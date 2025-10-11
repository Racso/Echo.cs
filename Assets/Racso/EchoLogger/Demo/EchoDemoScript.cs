#if UNITY_2017_1_OR_NEWER

using System.Collections;
using Racso.EchoLogger.Unity;
using UnityEngine;

namespace Racso.EchoLogger.Demo
{
    public class EchoDemoScript : MonoBehaviour
    {
        /* If you want to use visual tools (like the Echo Editor Window) to manage your log levels,
     you need to provide a list of your log systems so they can be shown.

     You can define your log systems as strings in a class. We suggest using a static class and readonly strings.
     This helps avoiding typos. By using hardcoded strings, we avoid generating garbage at runtime (which
     would happen if we used enums as we'd need to call ToString() on them). */
        public static class LogSystems
        {
            public static readonly string General = "General"; // You can also define a general system for logs that don't fit in any specific system.
            public static readonly string GUI = "GUI";
            public static readonly string Physics = "Physics";
            public static readonly string AI = "AI";
            public static readonly string Rendering = "Rendering";
            public static readonly string Animation = "Animation";
            public static readonly string Networking = "Networking";
        }

        IEnumerator Start()
        {
            // You can create your own custom LogWriter by implementing the EchoLogWriter interface...
            // EchoLogWriter writer = new MyCustomWriter();
            // Echo echo = new Echo(writer);

            // ..OR you can use a default built-in Unity writer:
            Echo echo = EchoUnity.New(); // Optional: a LogWriterConfig can be passed to customize the built-in writer.

            // Optional Unity integration: you can use the Editor and Runtime windows to manage logs.
            EchoUnity.SetupWindows(echo, typeof(LogSystems)); // Required to use Editor and Runtime windows.

            // Editor window (in the Unity Editor):
            // Open it from "Tools > Racso > Echo Logger Window".

            // Runtime window (in-game):
            EchoRuntimeWindow runtimeWindow = gameObject.AddComponent<EchoRuntimeWindow>(); // Add this component.
            runtimeWindow.Visible = true; // Use this to show/hide the runtime window from your code.

            while (true)
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.Space))
                    PrintLogs(echo);
            }
        }

        void PrintLogs(Echo echo)
        {
            EchoLogger logger = echo.GetLogger(); // Cached; always returns the same instance.
            logger.Debug(LogSystems.GUI, "This is a debug message from the GUI system.");
            logger.Info(LogSystems.Physics, "This is an info message from the Physics system.");
            logger.Warn(LogSystems.AI, "This is a warning message from the AI system.");
            logger.Error(LogSystems.Rendering, "This is an error message from the Rendering system.");

            EchoSystemLogger animationLogger = echo.GetSystemLogger(LogSystems.Animation); // Cached; always returns the same instance for "Animation".
            animationLogger.Debug("This is a debug message from the Animation system.");
            animationLogger.Info("This is an info message from the Animation system.");
            animationLogger.Warn("This is a warning message from the Animation system.");
            animationLogger.Error("This is an error message from the Animation system.");

            // To reduce garbage to a minimum, avoid using string interpolation or concatenation in log messages.
            // Instead, use formatted strings with parameters. Formatting is done only IF the log will be written.
            string playerName = "John";
            int playerHealth = Random.Range(0, 100);
            logger.Info(LogSystems.General, "Player {0} has {1} health.", playerName, playerHealth);

            // Optional (if using the EchoUnity integration):
            // The EchoUnity state is cleared on Playmode entry or Domain reload, but you can do it manually, too:
            EchoUnity.Clear();
        }

        private void OnDestroy()
        {
            EchoUnity.Clear(); // Optional (done automatically on Playmode entry or Domain reload).
        }
    }
}

#endif