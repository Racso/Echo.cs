using NUnit.Framework;

namespace Racso.EchoLogger.Tests
{
    [TestFixture]
    public class EchoLoggerTests
    {
        private MemoryLogWriter writer;
        private Echo echo;
        private EchoLogger logger;

        [SetUp]
        public void SetUp()
        {
            writer = new MemoryLogWriter();
            echo = new Echo(writer);
            logger = echo.GetLogger();
            
            // Set default level to Debug so all logs are written
            echo.Settings.SetDefaultLevel(LogLevel.Debug);
        }

        #region Debug Tests

        [Test]
        public void Debug_WritesLog_WithCorrectLevel()
        {
            logger.Debug("TestSystem", "Test message");

            Assert.AreEqual(1, writer.Count);
            Assert.AreEqual(LogLevel.Debug, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Debug_WritesLog_WithCorrectSystem()
        {
            logger.Debug("TestSystem", "Test message");

            Assert.AreEqual("TestSystem", writer.GetLogs()[0].System);
        }

        [Test]
        public void Debug_WritesLog_WithCorrectMessage()
        {
            logger.Debug("TestSystem", "Test message");

            Assert.AreEqual("Test message", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithOneParameter_FormatsMessage()
        {
            logger.Debug("TestSystem", "Value: {0}", 42);

            Assert.AreEqual("Value: 42", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithTwoParameters_FormatsMessage()
        {
            logger.Debug("TestSystem", "Name: {0}, Age: {1}", "John", 30);

            Assert.AreEqual("Name: John, Age: 30", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithThreeParameters_FormatsMessage()
        {
            logger.Debug("TestSystem", "{0} {1} {2}", "A", "B", "C");

            Assert.AreEqual("A B C", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug_WithFourParameters_FormatsMessage()
        {
            logger.Debug("TestSystem", "{0} {1} {2} {3}", "A", "B", "C", "D");

            Assert.AreEqual("A B C D", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Debug1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Debug1("TestSystem", "Same message");
            logger.Debug1("TestSystem", "Same message");
            logger.Debug1("TestSystem", "Same message");

            Assert.AreEqual(1, writer.Count);
        }

        [Test]
        public void Debug1_WritesMultipleTimes_ForDifferentMessages()
        {
            logger.Debug1("TestSystem", "Message 1");
            logger.Debug1("TestSystem", "Message 2");
            logger.Debug1("TestSystem", "Message 3");

            Assert.AreEqual(3, writer.Count);
        }

        [Test]
        public void Debug1_WithParameters_WritesOnlyOnce()
        {
            logger.Debug1("TestSystem", "Value: {0}", 42);
            logger.Debug1("TestSystem", "Value: {0}", 42);

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Info Tests

        [Test]
        public void Info_WritesLog_WithCorrectLevel()
        {
            logger.Info("TestSystem", "Test message");

            Assert.AreEqual(LogLevel.Info, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Info_WithOneParameter_FormatsMessage()
        {
            logger.Info("TestSystem", "Count: {0}", 100);

            Assert.AreEqual("Count: 100", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Info_WithTwoParameters_FormatsMessage()
        {
            logger.Info("TestSystem", "{0}: {1}", "Key", "Value");

            Assert.AreEqual("Key: Value", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Info1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Info1("TestSystem", "Same info");
            logger.Info1("TestSystem", "Same info");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Warn Tests

        [Test]
        public void Warn_WritesLog_WithCorrectLevel()
        {
            logger.Warn("TestSystem", "Test warning");

            Assert.AreEqual(LogLevel.Warn, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Warn_WithOneParameter_FormatsMessage()
        {
            logger.Warn("TestSystem", "Warning code: {0}", 404);

            Assert.AreEqual("Warning code: 404", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Warn1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Warn1("TestSystem", "Same warning");
            logger.Warn1("TestSystem", "Same warning");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Error Tests

        [Test]
        public void Error_WritesLog_WithCorrectLevel()
        {
            logger.Error("TestSystem", "Test error");

            Assert.AreEqual(LogLevel.Error, writer.GetLogs()[0].Level);
        }

        [Test]
        public void Error_WithOneParameter_FormatsMessage()
        {
            logger.Error("TestSystem", "Error code: {0}", 500);

            Assert.AreEqual("Error code: 500", writer.GetLogs()[0].Message);
        }

        [Test]
        public void Error1_WritesOnlyOnce_ForSameMessage()
        {
            logger.Error1("TestSystem", "Same error");
            logger.Error1("TestSystem", "Same error");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region Log Level Filtering Tests

        [Test]
        public void Debug_DoesNotWrite_WhenLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Info);
            logger.Debug("TestSystem", "Debug message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Info_DoesNotWrite_WhenLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Warn);
            logger.Info("TestSystem", "Info message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Warn_DoesNotWrite_WhenLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            logger.Warn("TestSystem", "Warn message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Error_DoesNotWrite_WhenLevelIsNone()
        {
            echo.Settings.SetDefaultLevel(LogLevel.None);
            logger.Error("TestSystem", "Error message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void Error_Writes_WhenLevelIsError()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            logger.Error("TestSystem", "Error message");

            Assert.AreEqual(1, writer.Count);
        }

        #endregion

        #region System-Specific Level Tests

        [Test]
        public void Debug_Writes_WhenSystemLevelIsDebug()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            echo.Settings.SetSystemLevel("TestSystem", LogLevel.Debug);

            logger.Debug("TestSystem", "Debug message");

            Assert.AreEqual(1, writer.Count);
        }

        [Test]
        public void Debug_DoesNotWrite_WhenSystemLevelIsTooHigh()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Debug);
            echo.Settings.SetSystemLevel("TestSystem", LogLevel.Error);

            logger.Debug("TestSystem", "Debug message");

            Assert.AreEqual(0, writer.Count);
        }

        [Test]
        public void MultipleSystems_RespectIndividualLevels()
        {
            echo.Settings.SetDefaultLevel(LogLevel.Error);
            echo.Settings.SetSystemLevel("System1", LogLevel.Debug);
            echo.Settings.SetSystemLevel("System2", LogLevel.Info);

            logger.Debug("System1", "Debug from System1");
            logger.Debug("System2", "Debug from System2");
            logger.Info("System1", "Info from System1");
            logger.Info("System2", "Info from System2");

            Assert.AreEqual(3, writer.Count); // System1 Debug, System1 Info, System2 Info
        }

        #endregion
    }
}
