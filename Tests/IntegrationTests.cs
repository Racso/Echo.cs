using NUnit.Framework;
using System.Linq;

namespace Racso.EchoLogger.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private MemoryLogWriter writer;
        private Echo echo;

        [SetUp]
        public void SetUp()
        {
            writer = new MemoryLogWriter();
            echo = new Echo(writer);
        }

        [Test]
        public void MultipleSystemsWithDifferentLevels_LogCorrectly()
        {
            // Set up different levels for different systems
            echo.Settings.SetSystemLevel("GUI", LogLevel.Debug);
            echo.Settings.SetSystemLevel("Physics", LogLevel.Info);
            echo.Settings.SetSystemLevel("AI", LogLevel.Warn);
            echo.Settings.SetSystemLevel("Audio", LogLevel.Error);

            var logger = echo.GetLogger();

            // Log messages at different levels for each system
            logger.Debug("GUI", "GUI debug message");
            logger.Info("GUI", "GUI info message");
            
            logger.Debug("Physics", "Physics debug message");
            logger.Info("Physics", "Physics info message");
            
            logger.Debug("AI", "AI debug message");
            logger.Warn("AI", "AI warn message");
            
            logger.Debug("Audio", "Audio debug message");
            logger.Error("Audio", "Audio error message");

            // Verify correct logs were written
            Assert.AreEqual(5, writer.Count);
            
            // GUI: Debug and Info should be logged
            Assert.IsTrue(writer.GetLogs().Any(log => log.System == "GUI" && log.Level == LogLevel.Debug));
            Assert.IsTrue(writer.GetLogs().Any(log => log.System == "GUI" && log.Level == LogLevel.Info));
            
            // Physics: Only Info should be logged
            Assert.IsFalse(writer.GetLogs().Any(log => log.System == "Physics" && log.Level == LogLevel.Debug));
            Assert.IsTrue(writer.GetLogs().Any(log => log.System == "Physics" && log.Level == LogLevel.Info));
            
            // AI: Only Warn should be logged
            Assert.IsFalse(writer.GetLogs().Any(log => log.System == "AI" && log.Level == LogLevel.Debug));
            Assert.IsTrue(writer.GetLogs().Any(log => log.System == "AI" && log.Level == LogLevel.Warn));
            
            // Audio: Only Error should be logged
            Assert.IsFalse(writer.GetLogs().Any(log => log.System == "Audio" && log.Level == LogLevel.Debug));
            Assert.IsTrue(writer.GetLogs().Any(log => log.System == "Audio" && log.Level == LogLevel.Error));
        }

        [Test]
        public void ChangingSettingsDuringExecution_AffectsSubsequentLogs()
        {
            var logger = echo.GetLogger();

            // Initial settings: only errors
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            
            logger.Debug("TestSystem", "Debug 1");
            logger.Info("TestSystem", "Info 1");
            logger.Error("TestSystem", "Error 1");
            
            Assert.AreEqual(1, writer.Count); // Only Error 1
            Assert.AreEqual("Error 1", writer.GetLogs()[0].Message);

            // Change to Info level
            echo.Settings.SetDefaultLevel(LogLevel.Info);
            
            logger.Debug("TestSystem", "Debug 2");
            logger.Info("TestSystem", "Info 2");
            logger.Error("TestSystem", "Error 2");
            
            Assert.AreEqual(3, writer.Count); // Error 1, Info 2, Error 2
            Assert.AreEqual("Info 2", writer.GetLogs()[1].Message);
            Assert.AreEqual("Error 2", writer.GetLogs()[2].Message);

            // Change to Debug level
            echo.Settings.SetDefaultLevel(LogLevel.Debug);
            
            logger.Debug("TestSystem", "Debug 3");
            logger.Info("TestSystem", "Info 3");
            
            Assert.AreEqual(5, writer.Count); // Previous 3 + Debug 3 + Info 3
            Assert.AreEqual("Debug 3", writer.GetLogs()[3].Message);
            Assert.AreEqual("Info 3", writer.GetLogs()[4].Message);
        }

        [Test]
        public void ChangingSystemLevels_AffectsSubsequentLogs()
        {
            var logger = echo.GetLogger();
            echo.Settings.SetDefaultLevel(LogLevel.Error);

            // Initial: System1 at Debug, System2 at Error
            echo.Settings.SetSystemLevel("System1", LogLevel.Debug);
            echo.Settings.SetSystemLevel("System2", LogLevel.Error);

            logger.Debug("System1", "System1 Debug 1");
            logger.Debug("System2", "System2 Debug 1");
            logger.Error("System1", "System1 Error 1");
            logger.Error("System2", "System2 Error 1");

            Assert.AreEqual(3, writer.Count); // System1 Debug 1, System1 Error 1, System2 Error 1

            // Change System2 to Debug
            echo.Settings.SetSystemLevel("System2", LogLevel.Debug);

            logger.Debug("System1", "System1 Debug 2");
            logger.Debug("System2", "System2 Debug 2");

            Assert.AreEqual(5, writer.Count); // Previous 3 + both Debug 2 messages
        }

        [Test]
        public void ClearingSystemLevel_RestoresDefaultLevel()
        {
            var logger = echo.GetLogger();
            echo.Settings.SetDefaultLevel(LogLevel.Info);

            // Set System1 to Debug
            echo.Settings.SetSystemLevel("System1", LogLevel.Debug);

            logger.Debug("System1", "Debug 1");
            Assert.AreEqual(1, writer.Count);

            // Clear System1 level, should revert to default (Info)
            echo.Settings.ClearSystemLevel("System1");

            logger.Debug("System1", "Debug 2");
            logger.Info("System1", "Info 1");

            Assert.AreEqual(2, writer.Count); // Debug 1 and Info 1, but not Debug 2
            Assert.AreEqual("Debug 1", writer.GetLogs()[0].Message);
            Assert.AreEqual("Info 1", writer.GetLogs()[1].Message);
        }

        [Test]
        public void MixedEchoLoggerAndSystemLogger_WithChangingSettings()
        {
            var generalLogger = echo.GetLogger();
            var systemLogger1 = echo.GetSystemLogger("System1");
            var systemLogger2 = echo.GetSystemLogger("System2");

            // Initial setup
            echo.Settings.SetDefaultLevel(LogLevel.Warn);
            echo.Settings.SetSystemLevel("System1", LogLevel.Debug);

            generalLogger.Debug("System1", "General Debug to System1");
            generalLogger.Info("System2", "General Info to System2");
            systemLogger1.Debug("System1 Debug");
            systemLogger2.Info("System2 Info");

            Assert.AreEqual(2, writer.Count); // General Debug to System1, System1 Debug

            // Change System2 level to Debug
            echo.Settings.SetSystemLevel("System2", LogLevel.Debug);

            generalLogger.Debug("System2", "General Debug to System2");
            systemLogger2.Debug("System2 Debug");

            Assert.AreEqual(4, writer.Count); // Previous 2 + both new System2 debugs
        }

        [Test]
        public void LogMode_Once_WorksAcrossDifferentLoggerInstances()
        {
            var generalLogger = echo.GetLogger();
            var systemLogger = echo.GetSystemLogger("TestSystem");

            echo.Settings.SetDefaultLevel(LogLevel.Debug);

            // Log the same message using both loggers
            generalLogger.Debug1("TestSystem", "Same message");
            systemLogger.Debug1("Same message");

            // Should only log once because it's the same system and message
            Assert.AreEqual(1, writer.Count);
        }

        [Test]
        public void LogMode_Once_IsScopedToSystemAndMessage()
        {
            var logger = echo.GetLogger();
            echo.Settings.SetDefaultLevel(LogLevel.Debug);

            // Same message, different systems
            logger.Debug1("System1", "Message");
            logger.Debug1("System2", "Message");

            // Should log twice because different systems
            Assert.AreEqual(2, writer.Count);

            // Different message, same system
            logger.Debug1("System1", "Message2");

            // Should log again
            Assert.AreEqual(3, writer.Count);

            // Same message and system again
            logger.Debug1("System1", "Message");

            // Should not log again
            Assert.AreEqual(3, writer.Count);
        }

        [Test]
        public void ComplexScenario_MultipleSystemsMultipleLoggers_ChangingSettings()
        {
            // Create multiple loggers
            var generalLogger = echo.GetLogger();
            var guiLogger = echo.GetSystemLogger("GUI");
            var physicsLogger = echo.GetSystemLogger("Physics");
            var aiLogger = echo.GetSystemLogger("AI");

            // Scenario 1: Default level is Warn
            echo.Settings.SetDefaultLevel(LogLevel.Warn);

            guiLogger.Debug("GUI Debug 1");
            physicsLogger.Info("Physics Info 1");
            aiLogger.Warn("AI Warn 1");

            Assert.AreEqual(1, writer.Count); // Only AI Warn 1

            // Scenario 2: Enable Debug for GUI only
            echo.Settings.SetSystemLevel("GUI", LogLevel.Debug);

            guiLogger.Debug("GUI Debug 2");
            physicsLogger.Info("Physics Info 2");
            aiLogger.Error("AI Error 1");

            Assert.AreEqual(3, writer.Count); // AI Warn 1, GUI Debug 2, AI Error 1

            // Scenario 3: Enable Info for Physics
            echo.Settings.SetSystemLevel("Physics", LogLevel.Info);

            guiLogger.Info("GUI Info 1");
            physicsLogger.Info("Physics Info 3");
            aiLogger.Debug("AI Debug 1");

            Assert.AreEqual(5, writer.Count); // Previous 3 + GUI Info 1 + Physics Info 3

            // Scenario 4: Change default to Debug
            echo.Settings.SetDefaultLevel(LogLevel.Debug);

            aiLogger.Debug("AI Debug 2");
            generalLogger.Debug("General", "General Debug 1");

            Assert.AreEqual(7, writer.Count); // Previous 5 + AI Debug 2 + General Debug 1

            // Scenario 5: Clear all system levels
            echo.Settings.ClearSystemLevels();

            guiLogger.Debug("GUI Debug 3");
            physicsLogger.Debug("Physics Debug 1");
            aiLogger.Debug("AI Debug 3");

            Assert.AreEqual(10, writer.Count); // All should log now with Debug default

            // Scenario 6: Disable logging for specific system
            echo.Settings.SetSystemLevel("GUI", LogLevel.None);

            guiLogger.Error("GUI Error 1"); // Should not log
            physicsLogger.Debug("Physics Debug 2");

            Assert.AreEqual(11, writer.Count); // Previous 10 + Physics Debug 2 only
        }

        [Test]
        public void ParameterizedLogging_WithFormattedMessages_MultipleTypes()
        {
            var logger = echo.GetLogger();
            echo.Settings.SetDefaultLevel(LogLevel.Debug);

            // Different parameter types
            logger.Info("System", "Int: {0}", 42);
            logger.Info("System", "String: {0}", "test");
            logger.Info("System", "Double: {0}", 3.14);
            logger.Info("System", "Bool: {0}", true);

            Assert.AreEqual(4, writer.Count);
            Assert.AreEqual("Int: 42", writer.GetLogs()[0].Message);
            Assert.AreEqual("String: test", writer.GetLogs()[1].Message);
            Assert.AreEqual("Double: 3.14", writer.GetLogs()[2].Message);
            Assert.AreEqual("Bool: True", writer.GetLogs()[3].Message);

            // Multiple parameters
            logger.Info("System", "{0} + {1} = {2}", 1, 2, 3);
            Assert.AreEqual("1 + 2 = 3", writer.GetLogs()[4].Message);
        }
    }
}
