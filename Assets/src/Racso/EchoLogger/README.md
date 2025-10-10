# EchoLogger

EchoLogger is a flexible logging library for general C# projects, with seamless Unity integration. It allows you to organize logs by system, control log levels, and use visual tools for log management in Unity.

## Features

- Structured logging with system tags
- Customizable log levels
- Visual log management via Unity Editor and Runtime windows
- Extensible with custom log writers
- Lightweight and garbage-free.

## Installation

Install EchoLogger via Git URL:

```
https://github.com/your-org/echologger.git
```

*(Replace with the actual URL when available)*

## Usage

### General C# Projects

1. Reference the EchoLogger library in your project.
2. Create an `Echo` instance and use it to log messages:

```csharp
using Racso.EchoLogger;

Echo echo = new Echo(new MyCustomWriter());
EchoLogger logger = echo.GetLogger();
logger.Info("SystemName", "Your log message");
```

### Unity Integration

1. Add the EchoLogger package to your Unity project.
2. Use the built-in Unity writer and setup visual tools:

```csharp
using Racso.EchoLogger.Unity;

Echo echo = EchoUnity.NewDefaultEcho();
EchoUnity.SetupWindows(echo, typeof(LogSystems));
EchoRuntimeWindow runtimeWindow = gameObject.AddComponent<EchoRuntimeWindow>();
runtimeWindow.Visible = true;
```

3. Log messages by system:

```csharp
logger.Debug(LogSystems.GUI, "Debug message for GUI");
```

## Log Systems

Define your log systems as static readonly strings to avoid typos and runtime garbage:

```csharp
public static class LogSystems
{
    public static readonly string GUI = "GUI";
    public static readonly string Physics = "Physics";
    public static readonly string AI = "AI";
    public static readonly string Rendering = "Rendering";
    public static readonly string Animation = "Animation";
    public static readonly string Networking = "Networking";
}
```

## Example (Unity)

Below is a sample Unity MonoBehaviour demonstrating EchoLogger usage:

```csharp
using System.Collections;
using Racso.EchoLogger.Unity;
using UnityEngine;

namespace Racso.EchoLogger.Demo
{
    public class EchoDemoScript : MonoBehaviour
    {
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
            Echo echo = EchoUnity.NewDefaultEcho();
            EchoUnity.SetupWindows(echo, typeof(LogSystems));
            EchoRuntimeWindow runtimeWindow = gameObject.AddComponent<EchoRuntimeWindow>();
            runtimeWindow.Visible = true;

            while (true)
            {
                yield return null;
                if (Input.GetKeyDown(KeyCode.Space))
                    PrintLogs(echo);
            }
        }

        void PrintLogs(Echo factory)
        {
            EchoLogger logger = factory.GetLogger();
            logger.Debug(LogSystems.GUI, "This is a debug message from the GUI system.");
            logger.Info(LogSystems.Physics, "This is an info message from the Physics system.");
            logger.Warn(LogSystems.AI, "This is a warning message from the AI system.");
            logger.Error(LogSystems.Rendering, "This is an error message from the Rendering system.");

            EchoSystemLogger animationLogger = factory.GetSystemLogger(LogSystems.Animation);
            animationLogger.Debug("This is a debug message from the Animation system.");
            animationLogger.Info("This is an info message from the Animation system.");
            animationLogger.Warn("This is a warning message from the Animation system.");
            animationLogger.Error("This is an error message from the Animation system.");

            EchoSystemLogger networkingLogger = factory.GetSystemLogger(LogSystems.Networking);
            networkingLogger.Debug("This is a debug message from the Networking system.");
            networkingLogger.Info("This is an info message from the Networking system.");
            networkingLogger.Warn("This is a warning message from the Networking system.");
            networkingLogger.Error("This is an error message from the Networking system.");
        }

        private void OnDestroy()
        {
            EchoUnity.Clear();
        }
    }
}
```

## Visual Tools (Unity)

- **Editor Window**: Manage log levels and view logs in the Unity Editor.
- **Runtime Window**: View and filter logs during play mode.

## License

MIT (or specify your license here)

