# API Comparison: C# vs TypeScript

This document shows how the Echo TypeScript API mirrors the original C# API.

## Creating an Echo Instance

### C#
```csharp
// With Console writer
Echo echo = EchoConsole.New();

// With custom writer
EchoLogWriter writer = new MyCustomWriter();
Echo echo = new Echo(writer);
```

### TypeScript
```typescript
// With Console writer
const echo = EchoConsole.new();

// With custom writer
const writer = new MyCustomWriter();
const echo = new Echo(writer);
```

## Getting Loggers

### C#
```csharp
EchoLogger logger = echo.GetLogger();
EchoSystemLogger animationLogger = echo.GetSystemLogger("Animation");
```

### TypeScript
```typescript
const logger = echo.getLogger();
const animationLogger = echo.getSystemLogger("Animation");
```

## Logging Messages

### C#
```csharp
logger.Debug("System", "Debug message");
logger.Info("System", "Info message");
logger.Warn("System", "Warning message");
logger.Error("System", "Error message");
```

### TypeScript
```typescript
logger.debug("System", "Debug message");
logger.info("System", "Info message");
logger.warn("System", "Warning message");
logger.error("System", "Error message");
```

## String Formatting

### C#
```csharp
string playerName = "John";
int playerHealth = 100;
logger.Info("General", "Player {0} has {1} health.", playerName, playerHealth);
```

### TypeScript
```typescript
const playerName = "John";
const playerHealth = 100;
logger.info("General", "Player {0} has {1} health.", playerName, playerHealth);
```

## System Logger

### C#
```csharp
EchoSystemLogger systemLogger = echo.GetSystemLogger("Physics");
systemLogger.Debug("Physics calculation complete");
systemLogger.Info("Velocity updated");
```

### TypeScript
```typescript
const systemLogger = echo.getSystemLogger("Physics");
systemLogger.debug("Physics calculation complete");
systemLogger.info("Velocity updated");
```

## Log Once

### C#
```csharp
logger.Debug1("System", "This will only appear once");
logger.Info1("System", "This will only appear once");
logger.Warn1("System", "This will only appear once");
logger.Error1("System", "This will only appear once");
```

### TypeScript
```typescript
logger.debug1("System", "This will only appear once");
logger.info1("System", "This will only appear once");
logger.warn1("System", "This will only appear once");
logger.error1("System", "This will only appear once");
```

## Settings

### C#
```csharp
echo.Settings.SetDefaultLevel(LogLevel.Warn);
echo.Settings.SetSystemLevel("Physics", LogLevel.Debug);
LogLevel level = echo.Settings.GetSystemLevel("Physics");
echo.Settings.ClearSystemLevel("Physics");
echo.Settings.ClearSystemLevels();
```

### TypeScript
```typescript
echo.settings.setDefaultLevel(LogLevel.Warn);
echo.settings.setSystemLevel("Physics", LogLevel.Debug);
const level = echo.settings.getSystemLevel("Physics");
echo.settings.clearSystemLevel("Physics");
echo.settings.clearSystemLevels();
```

## Custom Configuration

### C#
```csharp
var config = new LogWriterConfig
{
    Timestamp = true,
    LevelLabels = true,
    LevelColors = true,
    SystemColor = SystemColor.LabelAndMessage
};
Echo echo = EchoConsole.New(config);
```

### TypeScript
```typescript
const config = new LogWriterConfig();
config.timestamp = true;
config.levelLabels = true;
config.levelColors = true;
config.systemColor = SystemColor.LabelAndMessage;
const echo = EchoConsole.new(config);
```

## Custom Log Writer

### C#
```csharp
public class MyCustomWriter : EchoLogWriter
{
    public void WriteLog(LogLevel level, string system, string message)
    {
        Console.WriteLine($"[{level}] [{system}] {message}");
    }
}

EchoLogWriter writer = new MyCustomWriter();
Echo echo = new Echo(writer);
```

### TypeScript
```typescript
class MyCustomWriter implements EchoLogWriter {
    writeLog(level: LogLevel, system: string, message: string): void {
        console.log(`[${level}] [${system}] ${message}`);
    }
}

const writer = new MyCustomWriter();
const echo = new Echo(writer);
```

## Key Differences

1. **Naming Convention**: 
   - C# uses PascalCase for methods: `GetLogger()`, `SetDefaultLevel()`
   - TypeScript uses camelCase for methods: `getLogger()`, `setDefaultLevel()`

2. **Type Parameters**:
   - C# uses generic type parameters: `Debug<T1>(string system, string format, T1 param1)`
   - TypeScript uses rest parameters: `debug(system: string, format: string, ...params: any[])`

3. **Properties**:
   - C# uses property syntax: `echo.Settings`
   - TypeScript uses property syntax: `echo.settings`

4. **Events**:
   - C# uses event syntax: `settings.Updated += OnUpdated;`
   - TypeScript uses callback registration: `settings.onUpdated(onUpdated);`

5. **Object Initialization**:
   - C# uses object initializer syntax: `new LogWriterConfig { Timestamp = true }`
   - TypeScript uses property assignment after construction: `config.timestamp = true`

## Compatibility

The TypeScript port maintains **100% functional compatibility** with the C# version (excluding Unity-specific features). All core functionality works identically:

- ✅ Log levels and filtering
- ✅ System-based organization
- ✅ String formatting with parameters
- ✅ Log-once functionality
- ✅ Custom log writers
- ✅ Settings management
- ✅ Caching of logger instances

The only differences are syntactical, following TypeScript/JavaScript conventions.
