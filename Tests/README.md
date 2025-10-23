# Echo Library Unit Tests

This directory contains comprehensive unit tests for the Echo logging library.

## Test Structure

### Test Helper Classes

- **MemoryLogWriter.cs**: A test implementation of `EchoLogWriter` that stores logs in memory for validation. This enables tests to verify that expected logs are written correctly.

### Test Files

1. **EchoSettingsTests.cs** (23 tests)
   - Tests for log level management
   - Tests for system-specific levels
   - Tests for settings events
   - Tests for clearing and retrieving levels

2. **EchoTests.cs** (9 tests)
   - Tests for Echo initialization
   - Tests for logger creation and caching
   - Tests for GetLogger() and GetSystemLogger()
   - Tests for null/empty system name validation

3. **EchoLoggerTests.cs** (27 tests)
   - Tests for all log levels (Debug, Info, Warn, Error)
   - Tests for formatted messages with 1-4 parameters
   - Tests for LogMode.Once behavior
   - Tests for log level filtering
   - Tests for system-specific level overrides

4. **EchoSystemLoggerTests.cs** (24 tests)
   - Tests for all log levels (Debug, Info, Warn, Error)
   - Tests for formatted messages with 1-4 parameters
   - Tests for LogMode.Once behavior
   - Tests for log level filtering
   - Tests for multiple system loggers

5. **HelpersTests.cs** (9 tests)
   - Tests for FNV1a32 hash function
   - Tests for hash chaining
   - Tests for GetElementFromHash distribution

6. **IntegrationTests.cs** (13 tests)
   - Complex scenarios with multiple systems
   - Dynamic settings changes during execution
   - Tests combining EchoLogger and EchoSystemLogger
   - Tests for LogMode.Once across different logger instances
   - Tests with parametrized logging

## Total Coverage

- **105 tests** covering all major functionality
- **~1,300 lines** of test code
- All tests passing ✓

## Running the Tests

### In Unity
1. Open the project in Unity
2. Open Window > General > Test Runner
3. Select "PlayMode" or "EditMode" tab
4. Click "Run All"

### In .NET (for development)
The tests can also be compiled and run as a standalone .NET project using NUnit:
```bash
dotnet test
```

## Key Test Scenarios

The tests include several important scenarios as requested:

1. **Multiple Systems**: Tests verify that different systems can have different log levels and log independently
2. **Dynamic Settings Changes**: Tests verify that changing `EchoSettings` during execution affects subsequent logs
3. **LogMode.Once**: Tests verify that logs with `LogMode.Once` are only written once per unique system+message combination
4. **Level Filtering**: Tests verify that logs are filtered based on system-specific and default log levels
5. **Formatted Messages**: Tests verify that formatted messages with parameters work correctly

## Test Principles

- **Minimal dependencies**: Tests only depend on the Echo library and NUnit
- **Clear naming**: Test names clearly describe what is being tested
- **Isolation**: Each test is independent and uses a fresh Echo instance
- **Validation**: Tests use the MemoryLogWriter to verify exact log output
