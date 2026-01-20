using NUnit.Framework;
using System;

namespace Racso.EchoLogger.Tests
{
    [TestFixture]
    public class EchoTests
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
        public void Constructor_ThrowsException_WhenWriterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Echo(null));
        }

        [Test]
        public void GetLogger_ReturnsLogger()
        {
            var logger = echo.GetLogger();
            Assert.IsNotNull(logger);
        }

        [Test]
        public void GetLogger_ReturnsSameInstance_WhenCalledMultipleTimes()
        {
            var logger1 = echo.GetLogger();
            var logger2 = echo.GetLogger();
            Assert.AreSame(logger1, logger2);
        }

        [Test]
        public void GetSystemLogger_ReturnsLogger()
        {
            var logger = echo.GetSystemLogger("TestSystem");
            Assert.IsNotNull(logger);
        }

        [Test]
        public void GetSystemLogger_ReturnsSameInstance_WhenCalledMultipleTimes()
        {
            var logger1 = echo.GetSystemLogger("TestSystem");
            var logger2 = echo.GetSystemLogger("TestSystem");
            Assert.AreSame(logger1, logger2);
        }

        [Test]
        public void GetSystemLogger_ReturnsDifferentInstances_ForDifferentSystems()
        {
            var logger1 = echo.GetSystemLogger("System1");
            var logger2 = echo.GetSystemLogger("System2");
            Assert.AreNotSame(logger1, logger2);
        }

        [Test]
        public void GetSystemLogger_ThrowsException_WhenSystemIsNull()
        {
            Assert.Throws<ArgumentException>(() => echo.GetSystemLogger(null));
        }

        [Test]
        public void GetSystemLogger_ThrowsException_WhenSystemIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => echo.GetSystemLogger(""));
        }

        [Test]
        public void Settings_ReturnsEchoSettings()
        {
            Assert.IsNotNull(echo.Settings);
        }

        [Test]
        public void Settings_ReturnsSameInstance()
        {
            var settings1 = echo.Settings;
            var settings2 = echo.Settings;
            Assert.AreSame(settings1, settings2);
        }
    }
}
