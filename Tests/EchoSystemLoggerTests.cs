using NUnit.Framework;

namespace Racso.EchoLogger.Tests
{
    [TestFixture]
    public class EchoSystemLoggerTests
    {
        private MemoryLogWriter writer;
        private Echo echo;
        private EchoSystemLogger logger;

        [SetUp]
        public void SetUp()
        {
            writer = new MemoryLogWriter();
            echo = new Echo(writer);
            logger = echo.GetSystemLogger("TestSystem");
            
            // Set default level to Debug so all logs are written
            echo.Settings.SetDefaultLevel(LogLevel.Debug);
        }

        #region Debug Tests

        [Test]
        public void Debug_WritesLog_WithCorrectLevel()
        {
            logger.Debug("Test message");

            Assert.AreEqual(1, writer.Count);
            Assert.AreEqual(LogLevel.Debug, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Debug_WritesLog_WithCorrectSystem()
        {
            logger.Debug("Test message");

            Assert.AreEqual("TestSystem", writer.GetLogs()[0].System);
        }

        [Test]
        public void Debug_WritesLog_WithCorrectMessage()
        {
            logger.Debug("Test message");

            Assert.AreEqual("Test message", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithOneParameter_FormatsMessage()
        {
            logger.Debug("Value: {0}", 42);

            Assert.AreEqual("Value: 42", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithTwoParameters_FormatsMessage()
        {
            logger.Debug("Name: {0}, Age: {1}", "John", 30);

            Assert.AreEqual("Name: John, Age: 30", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithThreeParameters_FormatsMessage()
        {
            logger.Debug("{0} {1} {2}", "A", "B", "C");

            Assert.AreEqual("A B C", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithFourParameters_FormatsMessage()
        {
            logger.Debug("{0} {1} {2} {3}", "A", "B", "C", "D");

            Assert.AreEqual("A B C D", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Debug1("Same message");
            logger.Debug1("Same message");
            logger.Debug1("Same message");

            Assert.AreEqual(1, writer.Count);
        }

        [Test]
        public void Debug1_WritesMultipleTimes_ForDifferentMessages()
        {
            logger.Debug1("Message 1");
            logger.Debug1("Message 2");
            logger.Debug1("Message 3");

            Assert.AreEqual(3, writer.Count);
        }

        #endregion

        #region Info Tests

        [Test]
        public void Info_WritesLog_WithCorrectLevel()
        {
            logger.Info("Test message");

            Assert.AreEqual(LogLevel.Info, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Info_WithOneParameter_FormatsMessage()
        {
            logger.Info("Count: {0}", 100);

            Assert.AreEqual("Count: 100", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Info_WithTwoParameters_FormatsMessage()
        {
            logger.Info("{0}: {1}", "Key", "Value");

            Assert.AreEqual("Key: Value", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Info1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Info1("Same info");
            logger.Info1("Same info");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Warn Tests

        [Test]
        public void Warn_WritesLog_WithCorrectLevel()
        {
            logger.Warn("Test warning");

            Assert.AreEqual(LogLevel.Warn, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Warn_WithOneParameter_FormatsMessage()
        {
            logger.Warn("Warning code: {0}", 404);

            Assert.AreEqual("Warning code: 404", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Warn1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Warn1("Same warning");
            logger.Warn1("Same warning");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Error Tests

        [Test]
        public void Error_WritesLog_WithCorrectLevel()
        {
            logger.Error("Test error");

            Assert.AreEqual(LogLevel.Error, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Error_WithOneParameter_FormatsMessage()
        {
            logger.Error("Error code: {0}", 500);

            Assert.AreEqual("Error code: 500", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Error1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Error1("Same error");
            logger.Error1("Same error");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Log Level Filtering Tests

        [Test]
        public void Debug_DoesNotWrite_WhenLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Info);
            logger.Debug("Debug message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Info_DoesNotWrite_WhenLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Warn);
            logger.Info("Info message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Warn_DoesNotWrite_WhenLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            logger.Warn("Warn message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Error_DoesNotWrite_WhenLevelIsNone()
        {
            echo.Settings.SetDefaultLevel(LogLevel.None);
            logger.Error("Error message");

            Assert.AreEqual(0, writer.Count);
        }

        #endregion

        #region System-Specific Level Tests

        [Test]
        public void Debug_Writes_WhenSystemLevelIsDebug()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            echo.Settings.SetSystemLevel("TestSystem", LogLevel.Debug);

            logger.Debug("Debug message");

            Assert.AreEqual(1, writer.Count);
        }

        [Test]
        public void Debug_DoesNotWrite_WhenSystemLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Debug);
            echo.Settings.SetSystemLevel("TestSystem", LogLevel.Error);

            logger.Debug("Debug message");

            Assert.AreEqual(0, writer.Count);
        }

        #endregion

        #region Multiple System Loggers Tests

        [Test]
        public void MultipleSystemLoggers_WriteToCorrectSystems()
        {
            var logger1 = echo.GetSystemLogger("System1");
            var logger2 = echo.GetSystemLogger("System2");

            logger1.Info("Message from System1");
            logger2.Info("Message from System2");

            Assert.AreEqual(2, writer.Count);
            Assert.AreEqual("System1", writer.GetLogs()[0].System);
            Assert.AreEqual("System2", writer.GetLogs()[1].System);
        }

        #endregion
    }
}
