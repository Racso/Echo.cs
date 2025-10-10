using System.Collections;
using Racso.Echo;
using Racso.Echo.Unity;
using UnityEngine;

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
        // EchoFactory factory = new EchoFactory(writer);

        // Or you can use the default built-in Unity writer.
        EchoUnityFactory factory = new(); // Optional LogWriterConfig can be passed to customize the built-in writer.
        EchoSettings settings = factory.LogLevels;
        EchoUnity.Setup(settings, typeof(LogSystems)); // Required for the Unity Editor and Runtime windows.
        gameObject.AddComponent<EchoRuntimeWindow>(); // Enables the Echo Runtime Window
        EchoRuntimeWindow.Visible = true; // Call this to show/hide the runtime window from anywhere.

        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
                PrintLogs(factory);
        }
    }

    void PrintLogs(EchoFactory factory)
    {
        EchoLogger logger = factory.GetLogger(); // Cached; always returns the same instance.
        logger.Debug(LogSystems.GUI, "This is a debug message from the GUI system.");
        logger.Info(LogSystems.Physics, "This is an info message from the Physics system.");
        logger.Warn(LogSystems.AI, "This is a warning message from the AI system.");
        logger.Error(LogSystems.Rendering, "This is an error message from the Rendering system.");

        EchoSystemLogger systemLoggerA = factory.GetSystemLogger(LogSystems.Animation); // Cached; always returns the same instance for "Animation".
        systemLoggerA.Debug("This is a debug message from the Animation system.");
        systemLoggerA.Info("This is an info message from the Animation system.");
        systemLoggerA.Warn("This is a warning message from the Animation system.");
        systemLoggerA.Error("This is an error message from the Animation system.");

        EchoSystemLogger systemLoggerB = factory.GetSystemLogger(LogSystems.Networking); // Cached; always returns the same instance for "Networking".
        systemLoggerB.Debug("This is a debug message from the Networking system.");
        systemLoggerB.Info("This is an info message from the Networking system.");
        systemLoggerB.Warn("This is a warning message from the Networking system.");
        systemLoggerB.Error("This is an error message from the Networking system.");
    }

    private void OnDestroy()
    {
        EchoUnity.Clear(); // Optional (done automatically on Playmode entry or Domain reload).
    }
}