# Echo

Echo is a flexible logging library for general C# projects, with seamless Unity integration. It allows you to
organize logs by system, control log levels, and use visual tools for log management in Unity.

## Features

- Structured logging with system tags
- Customizable log levels
- Visual log management via Unity Editor and Runtime windows
- Extensible with custom log writers
- Lightweight and garbage-free.

## Installation

### Unity
Install Echo via Git URL (Package Manager > + > Add package from git URL):

```
https://github.com/Racso/toolbox.git?path=/Assets/Racso/EchoLogger#0.1.0
```

Reference the `Echo.asmdef` assembly definition in your scripts.

## Quick Reference

```csharp
    // == INITIALIZATION ==
    
    // An Echo instance is the main entry point to the library.
    
    // You can instantiate an Echo instance with default behavior for Unity...
    Echo echo = EchoUnity.New();
    
    // ...or with default behavior for Console applications:
    // Echo echo = EchoConsole.New();
    
    // ...or with a custom log writer to suit your specific needs (more on that below):
    // Echo echo = new Echo(new MyCustomWriter());
    
    // Optional Unity integration: you can use the Editor and Runtime windows to manage logs.
    EchoUnity.SetupWindows(echo, typeof(LogSystems)); // Required to use Editor and Runtime windows.
    
    // Editor window (in the Unity Editor):
    // Open it from "Tools > Racso > Echo Logger Window".
    
    // Runtime window (in-game):
    EchoRuntimeWindow runtimeWindow = gameObject.AddComponent<EchoRuntimeWindow>(); // Add this component.
    runtimeWindow.Visible = true; // Use this to show/hide the runtime window from your code.
    
    // == USAGE ==
    
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
```

## Customizing logs with Log Writers

Internally, Echo uses a LogWriter to output log messages. You can use built-in log writers or create your own custom ones.

Echo comes with 2 built-in log writers for Unity and Console applications:

- `EchoUnity`: Writes logs to the Unity Console.
- `EchoConsole`: Writes logs to the standard console output.

Use `EchoUnity.New()` or `EchoConsole.New()` to create an `Echo` instance with the respective built-in log writer.

Those methods also accept an optional `LogWriterConfig` parameter to customize log appearance:

- `Timestamp`: Include timestamps in log messages (default: true in Console, false in Unity)
- `LevelLabels`: Include log level labels (default: true in Console, false in Unity)
- `LevelColors`: Use colors for log levels (default: true)
- `SystemColor`: Controls system color usage:
    - `None`: No color
    - `LabelOnly`: Color only the system label (default)
    - `LabelAndMessage`: Color both label and message

### Custom Log Writers

You can create your own custom LogWriter by implementing the `EchoLogWriter` interface.

```csharp
    public interface EchoLogWriter
    {
        public void WriteLog(LogLevel level, string system, string message);
    }
    
    public class MyCustomWriter : EchoLogWriter
    {
        public void WriteLog(LogLevel level, string system, string message)
        {
            // Custom log writing logic here. Example:
            Console.WriteLine($"[{level}] [{system}] {message}");
        }
    }
    
    // Usage:
    EchoLogWriter writer = new MyCustomWriter();
    Echo echo = new Echo(writer);
```

## License

Copyright © 2025 Racso.

This software is part of the Racso Toolbox project: https://github.com/Racso/toolbox

Please refer to the Toolbox's README file for more information about licensing and terms of use.
