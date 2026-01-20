using NUnit.Framework;
using System;

namespace Racso.EchoLogger.Tests
{
    [TestFixture]
    public class EchoSettingsTests
    {
        private EchoSettings settings;

        [SetUp]
        public void SetUp()
        {
            settings = new EchoSettings();
        }

        [Test]
        public void DefaultLevel_IsWarn()
        {
            Assert.AreEqual(LogLevel.Warn, settings.DefaultLevel);
        }

        [Test]
        public void SetDefaultLevel_UpdatesDefaultLevel()
        {
            settings.SetDefaultLevel(LogLevel.Debug);
            Assert.AreEqual(LogLevel.Debug, settings.DefaultLevel);
        }

        [Test]
        public void SetDefaultLevel_RaisesUpdatedEvent()
        {
            bool eventRaised = false;
            settings.Updated += () => eventRaised = true;

            settings.SetDefaultLevel(LogLevel.Error);

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void GetSystemLevel_ReturnsDefaultLevel_WhenNotSet()
        {
            var level = settings.GetSystemLevel("TestSystem");
            Assert.AreEqual(LogLevel.Warn, level);
        }

        [Test]
        public void SetSystemLevel_SetsLevelForSystem()
        {
            settings.SetSystemLevel("TestSystem", LogLevel.Debug);
            var level = settings.GetSystemLevel("TestSystem");
            Assert.AreEqual(LogLevel.Debug, level);
        }

        [Test]
        public void SetSystemLevel_RaisesUpdatedEvent()
        {
            bool eventRaised = false;
            settings.Updated += () => eventRaised = true;

            settings.SetSystemLevel("TestSystem", LogLevel.Info);

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void SetSystemLevel_ThrowsException_WhenSystemIsNull()
        {
            Assert.Throws<ArgumentException>(() => settings.SetSystemLevel(null, LogLevel.Debug));
        }

        [Test]
        public void SetSystemLevel_ThrowsException_WhenSystemIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => settings.SetSystemLevel("", LogLevel.Debug));
        }

        [Test]
        public void ClearSystemLevel_RemovesSystemLevel()
        {
            settings.SetSystemLevel("TestSystem", LogLevel.Debug);
            settings.ClearSystemLevel("TestSystem");

            var level = settings.GetSystemLevel("TestSystem");
            Assert.AreEqual(LogLevel.Warn, level); // Should return default level
        }

        [Test]
        public void ClearSystemLevel_RaisesUpdatedEvent_WhenSystemExists()
        {
            settings.SetSystemLevel("TestSystem", LogLevel.Debug);
            
            bool eventRaised = false;
            settings.Updated += () => eventRaised = true;

            settings.ClearSystemLevel("TestSystem");

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void ClearSystemLevel_DoesNotRaiseUpdatedEvent_WhenSystemDoesNotExist()
        {
            bool eventRaised = false;
            settings.Updated += () => eventRaised = true;

            settings.ClearSystemLevel("NonExistentSystem");

            Assert.IsFalse(eventRaised);
        }

        [Test]
        public void ClearSystemLevel_ThrowsException_WhenSystemIsNull()
        {
            Assert.Throws<ArgumentException>(() => settings.ClearSystemLevel(null));
        }

        [Test]
        public void ClearSystemLevel_ThrowsException_WhenSystemIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => settings.ClearSystemLevel(""));
        }

        [Test]
        public void TryGetSystemLevel_ReturnsTrue_WhenSystemLevelIsSet()
        {
            settings.SetSystemLevel("TestSystem", LogLevel.Info);

            bool result = settings.TryGetSystemLevel("TestSystem", out LogLevel level);

            Assert.IsTrue(result);
            Assert.AreEqual(LogLevel.Info, level);
        }

        [Test]
        public void TryGetSystemLevel_ReturnsFalse_WhenSystemLevelIsNotSet()
        {
            bool result = settings.TryGetSystemLevel("TestSystem", out LogLevel level);

            Assert.IsFalse(result);
            Assert.AreEqual(default(LogLevel), level);
        }

        [Test]
        public void TryGetSystemLevel_ThrowsException_WhenSystemIsNull()
        {
            Assert.Throws<ArgumentException>(() => settings.TryGetSystemLevel(null, out _));
        }

        [Test]
        public void TryGetSystemLevel_ThrowsException_WhenSystemIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => settings.TryGetSystemLevel("", out _));
        }

        [Test]
        public void ClearSystemLevels_RemovesAllSystemLevels()
        {
            settings.SetSystemLevel("System1", LogLevel.Debug);
            settings.SetSystemLevel("System2", LogLevel.Info);
            settings.SetSystemLevel("System3", LogLevel.Error);

            settings.ClearSystemLevels();

            Assert.AreEqual(LogLevel.Warn, settings.GetSystemLevel("System1"));
            Assert.AreEqual(LogLevel.Warn, settings.GetSystemLevel("System2"));
            Assert.AreEqual(LogLevel.Warn, settings.GetSystemLevel("System3"));
        }

        [Test]
        public void ClearSystemLevels_RaisesUpdatedEvent()
        {
            bool eventRaised = false;
            settings.Updated += () => eventRaised = true;

            settings.ClearSystemLevels();

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void GetAllSystemLevels_ReturnsEmptyDictionary_WhenNoSystemsSet()
        {
            var allLevels = settings.GetAllSystemLevels();
            Assert.AreEqual(0, allLevels.Count);
        }

        [Test]
        public void GetAllSystemLevels_ReturnsAllSetSystemLevels()
        {
            settings.SetSystemLevel("System1", LogLevel.Debug);
            settings.SetSystemLevel("System2", LogLevel.Info);

            var allLevels = settings.GetAllSystemLevels();

            Assert.AreEqual(2, allLevels.Count);
            Assert.AreEqual(LogLevel.Debug, allLevels["System1"]);
            Assert.AreEqual(LogLevel.Info, allLevels["System2"]);
        }

        [Test]
        public void MultipleSystemLevels_WorkIndependently()
        {
            settings.SetSystemLevel("System1", LogLevel.Debug);
            settings.SetSystemLevel("System2", LogLevel.Error);

            Assert.AreEqual(LogLevel.Debug, settings.GetSystemLevel("System1"));
            Assert.AreEqual(LogLevel.Error, settings.GetSystemLevel("System2"));
            Assert.AreEqual(LogLevel.Warn, settings.GetSystemLevel("System3")); // Default
        }
    }
}
