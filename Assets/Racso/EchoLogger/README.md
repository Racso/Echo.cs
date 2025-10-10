# EchoLogger

EchoLogger is a flexible logging library for general C# projects, with seamless Unity integration. It allows you to
organize logs by system, control log levels, and use visual tools for log management in Unity.

## Features

- Structured logging with system tags
- Customizable log levels
- Visual log management via Unity Editor and Runtime windows
- Extensible with custom log writers
- Lightweight and garbage-free.

## Installation

Unity: install EchoLogger via Git URL (Package Manager > + > Add package from git URL):

```
https://github.com/Racso/toolbox.git?path=/Assets/Racso/EchoLogger#0.1.0
```

Reference the `Echo.asmdef` assembly definition in your scripts.

## Quick Reference

```csharp
    // INITIALIZATION
    
    // You can create your own custom LogWriter by implementing the EchoLogWriter interface...
    // EchoLogWriter writer = new MyCustomWriter();
    // Echo echo = new Echo(writer);
    
    // ..OR you can use a default built-in Unity writer:
    Echo echo = EchoUnity.NewDefaultEcho(); // Optional: a LogWriterConfig can be passed to customize the built-in writer.
    
    // Optional Unity integration: you can use the Editor and Runtime windows to manage logs.
    EchoUnity.SetupWindows(echo, typeof(LogSystems)); // Required to use Editor and Runtime windows.
    
    // Editor window (in the Unity Editor):
    // Open it from "Tools > Racso > Echo Logger Window".
    
    // Runtime window (in-game):
    EchoRuntimeWindow runtimeWindow = gameObject.AddComponent<EchoRuntimeWindow>(); // Add this component.
    runtimeWindow.Visible = true; // Use this to show/hide the runtime window from your code.
    
    // USAGE:
    
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

## License

Copyright © 2025 Racso.

This software is part of the Racso Toolbox project: https://github.com/Racso/toolbox 

Please refer to the Toolbox's README file for more information about licensing and terms of use.


