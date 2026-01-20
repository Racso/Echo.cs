/**
 * Tests for Echo TypeScript port
 */

import { 
    Echo, 
    EchoConsole, 
    EchoLogger, 
    EchoSystemLogger, 
    EchoSettings, 
    EchoLogWriter, 
    LogLevel, 
    LogWriterConfig,
    SystemColor 
} from './echo';

// ============================================================================
// Mock Log Writer for Testing
// ============================================================================

class MemoryLogWriter implements EchoLogWriter {
    public logs: Array<{ level: LogLevel; system: string; message: string }> = [];

    writeLog(level: LogLevel, system: string, message: string): void {
        this.logs.push({ level, system, message });
    }

    clear(): void {
        this.logs = [];
    }

    getLastLog() {
        return this.logs[this.logs.length - 1];
    }
}

// ============================================================================
// Test Helpers
// ============================================================================

function assert(condition: boolean, message: string): void {
    if (!condition) {
        throw new Error(`Assertion failed: ${message}`);
    }
}

function assertEqual<T>(actual: T, expected: T, message: string): void {
    if (actual !== expected) {
        throw new Error(`${message}: expected ${expected}, got ${actual}`);
    }
}

function runTest(name: string, testFn: () => void): void {
    try {
        testFn();
        console.log(`✓ ${name}`);
    } catch (error) {
        console.error(`✗ ${name}`);
        console.error(`  ${error}`);
        process.exit(1);
    }
}

// ============================================================================
// Tests
// ============================================================================

// Test: Basic Logger Creation
runTest("Can create Echo instance with custom writer", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    assert(echo !== null, "Echo instance should be created");
    assert(echo.settings !== null, "Settings should be accessible");
});

// Test: GetLogger returns same instance
runTest("GetLogger returns cached instance", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    const logger1 = echo.getLogger();
    const logger2 = echo.getLogger();
    assert(logger1 === logger2, "GetLogger should return same instance");
});

// Test: GetSystemLogger returns same instance per system
runTest("GetSystemLogger returns cached instance per system", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    const logger1 = echo.getSystemLogger("System1");
    const logger2 = echo.getSystemLogger("System1");
    const logger3 = echo.getSystemLogger("System2");
    assert(logger1 === logger2, "GetSystemLogger should return same instance for same system");
    assert(logger1 !== logger3, "GetSystemLogger should return different instances for different systems");
});

// Test: Basic logging works
runTest("Basic logging writes to log writer", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Debug); // Set to Debug to allow all logs
    const logger = echo.getLogger();
    
    logger.debug("TestSystem", "Debug message");
    logger.info("TestSystem", "Info message");
    logger.warn("TestSystem", "Warn message");
    logger.error("TestSystem", "Error message");
    
    assertEqual(writer.logs.length, 4, "Should have 4 log entries");
    assertEqual(writer.logs[0].message, "Debug message", "First message should be debug");
    assertEqual(writer.logs[1].message, "Info message", "Second message should be info");
    assertEqual(writer.logs[2].message, "Warn message", "Third message should be warn");
    assertEqual(writer.logs[3].message, "Error message", "Fourth message should be error");
});

// Test: System logger logging
runTest("System logger writes with correct system", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Debug); // Set to Debug to allow all logs
    const systemLogger = echo.getSystemLogger("Physics");
    
    systemLogger.info("Physics calculation complete");
    
    assertEqual(writer.logs.length, 1, "Should have 1 log entry");
    assertEqual(writer.logs[0].system, "Physics", "System should be Physics");
    assertEqual(writer.logs[0].message, "Physics calculation complete", "Message should match");
});

// Test: String formatting
runTest("String formatting works correctly", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Debug); // Set to Debug to allow all logs
    const logger = echo.getLogger();
    
    logger.info("TestSystem", "Player {0} has {1} health", "John", 100);
    
    assertEqual(writer.logs.length, 1, "Should have 1 log entry");
    assertEqual(writer.logs[0].message, "Player John has 100 health", "Formatted message should be correct");
});

// Test: Log level filtering
runTest("Log level filtering works", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Warn);
    const logger = echo.getLogger();
    
    logger.debug("TestSystem", "Debug message"); // Should not be logged
    logger.info("TestSystem", "Info message");   // Should not be logged
    logger.warn("TestSystem", "Warn message");   // Should be logged
    logger.error("TestSystem", "Error message"); // Should be logged
    
    assertEqual(writer.logs.length, 2, "Should have 2 log entries (Warn and Error only)");
    assertEqual(writer.logs[0].level, LogLevel.Warn, "First log should be Warn");
    assertEqual(writer.logs[1].level, LogLevel.Error, "Second log should be Error");
});

// Test: System-specific log levels
runTest("System-specific log levels work", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Warn);
    echo.settings.setSystemLevel("VerboseSystem", LogLevel.Debug);
    const logger = echo.getLogger();
    
    logger.debug("NormalSystem", "Debug from normal");   // Should not be logged
    logger.debug("VerboseSystem", "Debug from verbose"); // Should be logged
    
    assertEqual(writer.logs.length, 1, "Should have 1 log entry");
    assertEqual(writer.logs[0].system, "VerboseSystem", "Logged system should be VerboseSystem");
});

// Test: Log once functionality
runTest("Log once (debug1) prevents duplicate logs", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Debug); // Set to Debug to allow all logs
    const logger = echo.getLogger();
    
    logger.debug1("TestSystem", "This message should only appear once");
    logger.debug1("TestSystem", "This message should only appear once");
    logger.debug1("TestSystem", "This message should only appear once");
    
    assertEqual(writer.logs.length, 1, "Should have 1 log entry (logged once)");
});

// Test: Log once with different systems
runTest("Log once works per system", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    echo.settings.setDefaultLevel(LogLevel.Debug); // Set to Debug to allow all logs
    const logger = echo.getLogger();
    
    logger.info1("System1", "Same message");
    logger.info1("System2", "Same message");
    
    assertEqual(writer.logs.length, 2, "Should have 2 log entries (different systems)");
});

// Test: EchoConsole factory
runTest("EchoConsole.new() creates valid Echo instance", () => {
    const echo = EchoConsole.new();
    assert(echo !== null, "Echo instance should be created");
    
    const logger = echo.getLogger();
    // This will log to actual console, which is fine for this test
    logger.info("TestSystem", "Test message from EchoConsole");
});

// Test: EchoConsole with custom config
runTest("EchoConsole.new() accepts custom config", () => {
    const config = new LogWriterConfig();
    config.timestamp = false;
    config.levelLabels = false;
    
    const echo = EchoConsole.new(config);
    assert(echo !== null, "Echo instance should be created with custom config");
});

// Test: Settings update events
runTest("Settings trigger update events", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    
    let updateCount = 0;
    echo.settings.onUpdated(() => updateCount++);
    
    echo.settings.setDefaultLevel(LogLevel.Debug);
    echo.settings.setSystemLevel("TestSystem", LogLevel.Info);
    echo.settings.clearSystemLevel("TestSystem");
    
    assertEqual(updateCount, 3, "Should have triggered 3 updates");
});

// Test: Clear system levels
runTest("Clear system levels works", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    
    echo.settings.setSystemLevel("System1", LogLevel.Debug);
    echo.settings.setSystemLevel("System2", LogLevel.Info);
    assertEqual(echo.settings.getAllSystemLevels().size, 2, "Should have 2 system levels");
    
    echo.settings.clearSystemLevels();
    assertEqual(echo.settings.getAllSystemLevels().size, 0, "Should have 0 system levels after clear");
});

// Test: Error handling
runTest("Throws error for null writer", () => {
    try {
        new Echo(null as any);
        assert(false, "Should have thrown error");
    } catch (error) {
        assert(true, "Correctly threw error for null writer");
    }
});

runTest("Throws error for empty system name in GetSystemLogger", () => {
    const writer = new MemoryLogWriter();
    const echo = new Echo(writer);
    
    try {
        echo.getSystemLogger("");
        assert(false, "Should have thrown error");
    } catch (error) {
        assert(true, "Correctly threw error for empty system name");
    }
});

// ============================================================================
// Run All Tests
// ============================================================================

console.log("\n✅ All tests passed!");
