using System.Collections;
using Racso.Echo;
using Racso.Echo.Editor;
using UnityEngine;

public class EchoDemoScript : MonoBehaviour
{
    IEnumerator Start()
    {
        // You can create your own custom LogWriter by implementing the EchoLogWriter interface.
        // EchoLogWriter writer = new MyCustomWriter();
        // EchoFactory factory = new EchoFactory(writer);

        // Or you can use the default built-in writer.
        EchoFactory factory = new();
        EchoSettings settings = factory.LogLevels;
        EchoEditorState.Settings = settings; // Optional: only needed to edit levels in the Echo Editor Window.

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
        logger.Debug("GUI", "This is a debug message from the GUI system.");
        logger.Info("Physics", "This is an info message from the Physics system.");
        logger.Warn("AI", "This is a warning message from the AI system.");
        logger.Error("Rendering", "This is an error message from the Rendering system.");

        EchoSystemLogger systemLoggerA = factory.GetSystemLogger("Animation"); // Cached; always returns the same instance for "Animation".
        systemLoggerA.Debug("This is a debug message from the Animation system.");
        systemLoggerA.Info("This is an info message from the Animation system.");
        systemLoggerA.Warn("This is a warning message from the Animation system.");
        systemLoggerA.Error("This is an error message from the Animation system.");

        EchoSystemLogger systemLoggerB = factory.GetSystemLogger("Networking"); // Cached; always returns the same instance for "Networking".
        systemLoggerB.Debug("This is a debug message from the Networking system.");
        systemLoggerB.Info("This is an info message from the Networking system.");
        systemLoggerB.Warn("This is a warning message from the Networking system.");
        systemLoggerB.Error("This is an error message from the Networking system.");
    }
}