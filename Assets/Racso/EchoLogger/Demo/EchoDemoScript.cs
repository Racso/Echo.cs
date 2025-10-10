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
            public static readonly string GUI = "GUI";
            public static readonly string Physics = "Physics";
            public static readonly string AI = "AI";
            public static readonly string Rendering = "Rendering";
            public static readonly string Animation = "Animation";
            public static readonly string Networking = "Networking";
        }

        IEnumerator Start()
        {
            // You can create your own custom LogWriter by implementing the EchoLogWriter interface.
            // EchoLogWriter writer = new MyCustomWriter();
            // Echo echo = new Echo(writer);

            // Or you can use a default built-in Unity writer:
            Echo echo = EchoUnity.NewDefaultEcho(); // Optional LogWriterConfig can be passed to customize the built-in writer.

            // Optionally, use the Editor and Runtime windows to manage logs:
            EchoUnity.SetupWindows(echo, typeof(LogSystems)); // Required to use Editor and Runtime windows.
            EchoRuntimeWindow runtimeWindow = gameObject.AddComponent<EchoRuntimeWindow>(); // Add a Runtime Window.
            runtimeWindow.Visible = true; // Use this to show/hide the runtime window from your code.

            while (true)
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.Space))
                    PrintLogs(echo);
            }
        }

        void PrintLogs(Echo factory)
        {
            EchoLogger logger = factory.GetLogger(); // Cached; always returns the same instance.
            logger.Debug(LogSystems.GUI, "This is a debug message from the GUI system.");
            logger.Info(LogSystems.Physics, "This is an info message from the Physics system.");
            logger.Warn(LogSystems.AI, "This is a warning message from the AI system.");
            logger.Error(LogSystems.Rendering, "This is an error message from the Rendering system.");

            EchoSystemLogger animationLogger = factory.GetSystemLogger(LogSystems.Animation); // Cached; always returns the same instance for "Animation".
            animationLogger.Debug("This is a debug message from the Animation system.");
            animationLogger.Info("This is an info message from the Animation system.");
            animationLogger.Warn("This is a warning message from the Animation system.");
            animationLogger.Error("This is an error message from the Animation system.");

            EchoSystemLogger networkingLogger = factory.GetSystemLogger(LogSystems.Networking); // Cached; always returns the same instance for "Networking".
            networkingLogger.Debug("This is a debug message from the Networking system.");
            networkingLogger.Info("This is an info message from the Networking system.");
            networkingLogger.Warn("This is a warning message from the Networking system.");
            networkingLogger.Error("This is an error message from the Networking system.");
        }

        private void OnDestroy()
        {
            EchoUnity.Clear(); // Optional (done automatically on Playmode entry or Domain reload).
        }
    }
}