# Echo

A flexible and powerful logging library with structured logging, customizable log levels, and extensible log writers. Available for C# and TypeScript with nearly identical APIs.

## Features

- **Structured logging** with system tags for organized log output
- **Customizable log levels** (per-system or global): Debug, Info, Warn, Error
- **String formatting** with parameters (only formatted when log will be written)
- **Log-once functionality** to prevent duplicate messages
- **Extensible** with custom log writers
- **Performance optimized** - garbage-free when logs are filtered out
- **Type-safe** APIs in both languages

## Quick Start

### C#

```csharp
// Create an Echo instance
Echo echo = EchoConsole.New();

// Get a logger
EchoLogger logger = echo.GetLogger();

// Log messages
logger.Debug("System", "Debug message");
logger.Info("System", "Info message");
logger.Warn("System", "Warning message");
logger.Error("System", "Error message");

// Use string formatting (only formats if log will be written)
logger.Info("Physics", "Player {0} has {1} health", playerName, health);

// Get a system-specific logger
EchoSystemLogger physicsLogger = echo.GetSystemLogger("Physics");
physicsLogger.Debug("Velocity updated");
```

### TypeScript

```typescript
// Create an Echo instance
const echo = EchoConsole.new();

// Get a logger
const logger = echo.getLogger();

// Log messages
logger.debug("System", "Debug message");
logger.info("System", "Info message");
logger.warn("System", "Warning message");
logger.error("System", "Error message");

// Use string formatting (only formats if log will be written)
logger.info("Physics", "Player {0} has {1} health", playerName, health);

// Get a system-specific logger
const physicsLogger = echo.getSystemLogger("Physics");
physicsLogger.debug("Velocity updated");
```

## API Similarity

The TypeScript port maintains nearly identical API signatures to the C# version. The main differences are:

| Feature | C# | TypeScript |
|---------|-----|-----------|
| Method names | PascalCase (`GetLogger()`) | camelCase (`getLogger()`) |
| Properties | PascalCase (`Settings`) | camelCase (`settings`) |
| Generic methods | `Debug<T1>(...)` | Optional parameters `debug(..., param1?)` |
| Events | C# events (`Updated += handler`) | Callbacks (`onUpdated(handler)`) |

### Side-by-Side Comparison

```csharp
// C#
Echo echo = EchoConsole.New();
echo.Settings.SetDefaultLevel(LogLevel.Warn);
EchoLogger logger = echo.GetLogger();
logger.Info("System", "Player {0} health", health);
```

```typescript
// TypeScript
const echo = EchoConsole.new();
echo.settings.setDefaultLevel(LogLevel.Warn);
const logger = echo.getLogger();
logger.info("System", "Player {0} health", health);
```

## Installation

### C# (.NET / Unity)

For Unity projects, install via Git URL in Package Manager:
```
https://github.com/Racso/Echo.cs.git
```

For other .NET projects, copy the source files from the `CSharp/` directory.

See [CSharp/README.md](CSharp/README.md) for detailed C# documentation.

### TypeScript

Copy the TypeScript source from the `TypeScript/` directory or use the compiled version from `TypeScript/dist/`.

```bash
# Install dependencies
npm install --save-dev typescript @types/node

# Compile
cd TypeScript
npm run build
```

See [TypeScript/README.md](TypeScript/README.md) for detailed TypeScript documentation.

## Documentation

- **[CSharp/README.md](CSharp/README.md)** - C# version documentation (includes Unity integration)
- **[TypeScript/README.md](TypeScript/README.md)** - TypeScript version documentation
- **[API-COMPARISON.md](API-COMPARISON.md)** - Detailed API comparison between C# and TypeScript

## Core Concepts

### Log Levels

Echo supports four log levels (from most to least verbose):
- **Debug**: Detailed diagnostic information
- **Info**: General informational messages
- **Warn**: Warning messages for potentially harmful situations
- **Error**: Error messages for failures

Log levels can be set globally or per-system:

```csharp
// C#
echo.Settings.SetDefaultLevel(LogLevel.Warn);  // Global default
echo.Settings.SetSystemLevel("Physics", LogLevel.Debug);  // System-specific
```

```typescript
// TypeScript
echo.settings.setDefaultLevel(LogLevel.Warn);  // Global default
echo.settings.setSystemLevel("Physics", LogLevel.Debug);  // System-specific
```

### System-based Logging

Organize logs by system/component for better filtering and control:

```csharp
// C# - Using EchoLogger (specify system each time)
logger.Info("Physics", "Collision detected");
logger.Info("AI", "Pathfinding complete");

// C# - Using EchoSystemLogger (system set once)
EchoSystemLogger physicsLogger = echo.GetSystemLogger("Physics");
physicsLogger.Info("Collision detected");
```

```typescript
// TypeScript - Using EchoLogger (specify system each time)
logger.info("Physics", "Collision detected");
logger.info("AI", "Pathfinding complete");

// TypeScript - Using EchoSystemLogger (system set once)
const physicsLogger = echo.getSystemLogger("Physics");
physicsLogger.info("Collision detected");
```

### Custom Log Writers

Create custom log writers by implementing the `EchoLogWriter` interface:

```csharp
// C#
public class FileLogWriter : EchoLogWriter
{
    public void WriteLog(LogLevel level, string system, string message)
    {
        File.AppendAllText("log.txt", $"{DateTime.Now} [{level}] [{system}] {message}\n");
    }
}

Echo echo = new Echo(new FileLogWriter());
```

```typescript
// TypeScript
class FileLogWriter implements EchoLogWriter {
    writeLog(level: LogLevel, system: string, message: string): void {
        fs.appendFileSync('log.txt', `${new Date()} [${level}] [${system}] ${message}\n`);
    }
}

const echo = new Echo(new FileLogWriter());
```

## Performance

Both implementations are optimized for performance:
- **No allocations** when logs are filtered out
- **Lazy formatting** - string formatting only occurs when the log will be written
- **Instance caching** - loggers are cached and reused

## License

Copyright © 2025 Racso.

This software is part of the Racso Toolbox project: https://github.com/Racso/toolbox

Please refer to the Toolbox's README file for more information about licensing and terms of use.
