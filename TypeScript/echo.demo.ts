/**
 * Echo TypeScript Demo
 * 
 * This demonstrates how to use the Echo TypeScript library.
 */

import { Echo, EchoConsole, EchoLogger, EchoSystemLogger, LogLevel, LogWriterConfig, SystemColor } from './echo';

// ============================================================================
// Define your log systems
// ============================================================================

// You can define your log systems as string constants.
// This helps avoiding typos. By using hardcoded strings, we avoid generating 
// garbage at runtime (which would happen if we used enums as we'd need to 
// call toString() on them).
class LogSystems {
    static readonly General = "General";
    static readonly GUI = "GUI";
    static readonly Physics = "Physics";
    static readonly AI = "AI";
    static readonly Rendering = "Rendering";
    static readonly Animation = "Animation";
    static readonly Networking = "Networking";
}

// ============================================================================
// Basic Usage with Console Logger
// ============================================================================

function basicUsageDemo() {
    console.log("\n=== Basic Usage Demo ===\n");

    // Create an Echo instance with default console writer
    const echo = EchoConsole.new();

    // Get the main logger (cached; always returns the same instance)
    const logger = echo.getLogger();
    
    // Log messages with different levels and systems
    logger.debug(LogSystems.GUI, "This is a debug message from the GUI system.");
    logger.info(LogSystems.Physics, "This is an info message from the Physics system.");
    logger.warn(LogSystems.AI, "This is a warning message from the AI system.");
    logger.error(LogSystems.Rendering, "This is an error message from the Rendering system.");

    // Get a system logger (cached; always returns the same instance for "Animation")
    const animationLogger = echo.getSystemLogger(LogSystems.Animation);
    animationLogger.debug("This is a debug message from the Animation system.");
    animationLogger.info("This is an info message from the Animation system.");
    animationLogger.warn("This is a warning message from the Animation system.");
    animationLogger.error("This is an error message from the Animation system.");

    // Use formatted strings with parameters
    // Formatting is done only IF the log will be written (performance optimization)
    const playerName = "John";
    const playerHealth = Math.floor(Math.random() * 100);
    logger.info(LogSystems.General, "Player {0} has {1} health.", playerName, playerHealth);
}

// ============================================================================
// Custom Configuration Demo
// ============================================================================

function customConfigDemo() {
    console.log("\n=== Custom Configuration Demo ===\n");

    // Create a custom configuration
    const config = new LogWriterConfig();
    config.timestamp = true;
    config.levelLabels = true;
    config.levelColors = true;
    config.systemColor = SystemColor.LabelAndMessage; // Color both label and message

    // Create Echo with custom configuration
    const echo = EchoConsole.new(config);
    const logger = echo.getLogger();

    logger.info(LogSystems.Physics, "Physics simulation started");
    logger.info(LogSystems.Networking, "Connected to server");
    logger.info(LogSystems.GUI, "UI initialized");
}

// ============================================================================
// Log Level Management Demo
// ============================================================================

function logLevelDemo() {
    console.log("\n=== Log Level Management Demo ===\n");

    const echo = EchoConsole.new();
    const logger = echo.getLogger();

    // Set default log level to Warn (only Warn and Error will be logged)
    echo.settings.setDefaultLevel(LogLevel.Warn);
    console.log("Default level set to Warn - only Warn and Error should appear:\n");

    logger.debug(LogSystems.General, "This debug message won't appear");
    logger.info(LogSystems.General, "This info message won't appear");
    logger.warn(LogSystems.General, "This warning WILL appear");
    logger.error(LogSystems.General, "This error WILL appear");

    // Set system-specific log level
    echo.settings.setSystemLevel(LogSystems.Physics, LogLevel.Debug);
    console.log("\nPhysics system level set to Debug - all levels should appear for Physics:\n");

    logger.debug(LogSystems.Physics, "Physics debug message WILL appear");
    logger.debug(LogSystems.General, "General debug message still won't appear");
}

// ============================================================================
// Log Once Demo
// ============================================================================

function logOnceDemo() {
    console.log("\n=== Log Once Demo ===\n");

    const echo = EchoConsole.new();
    const logger = echo.getLogger();
    echo.settings.setDefaultLevel(LogLevel.Debug);

    console.log("Calling debug1() three times - should only log once:\n");

    // These will only log once, even though called multiple times
    logger.debug1(LogSystems.General, "This message should only appear once");
    logger.debug1(LogSystems.General, "This message should only appear once");
    logger.debug1(LogSystems.General, "This message should only appear once");

    console.log("\nCalling info1() with different systems - each system logs once:\n");

    // Log once works per system
    logger.info1(LogSystems.Physics, "Physics initialized");
    logger.info1(LogSystems.Networking, "Physics initialized"); // Different system, will log
    logger.info1(LogSystems.Physics, "Physics initialized"); // Same system+message, won't log
}

// ============================================================================
// Custom Log Writer Demo
// ============================================================================

import { EchoLogWriter } from './echo';

class CustomLogWriter implements EchoLogWriter {
    writeLog(level: LogLevel, system: string, message: string): void {
        // Custom log writing logic
        const levelName = ["NONE", "ERROR", "WARN", "INFO", "DEBUG"][level];
        const timestamp = new Date().toISOString();
        console.log(`${timestamp} | ${levelName} | [${system}] ${message}`);
    }
}

function customWriterDemo() {
    console.log("\n=== Custom Log Writer Demo ===\n");

    // Create Echo with custom log writer
    const customWriter = new CustomLogWriter();
    const echo = new Echo(customWriter);
    echo.settings.setDefaultLevel(LogLevel.Debug);
    
    const logger = echo.getLogger();
    logger.info(LogSystems.General, "Using custom log writer");
    logger.warn(LogSystems.AI, "Custom formatting applied");
}

// ============================================================================
// Run All Demos
// ============================================================================

function main() {
    console.log("╔═══════════════════════════════════════════════════════╗");
    console.log("║         Echo TypeScript Library Demo                 ║");
    console.log("╚═══════════════════════════════════════════════════════╝");

    basicUsageDemo();
    customConfigDemo();
    logLevelDemo();
    logOnceDemo();
    customWriterDemo();

    console.log("\n✅ Demo completed!\n");
}

main();
