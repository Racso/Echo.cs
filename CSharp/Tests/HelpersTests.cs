using NUnit.Framework;

namespace Racso.EchoLogger.Tests
{
    [TestFixture]
    public class HelpersTests
    {
        [Test]
        public void FNV1a32_ReturnsDifferentHashes_ForDifferentStrings()
        {
            uint hash1 = Helpers.FNV1a32("test1");
            uint hash2 = Helpers.FNV1a32("test2");

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void FNV1a32_ReturnsSameHash_ForSameString()
        {
            uint hash1 = Helpers.FNV1a32("test");
            uint hash2 = Helpers.FNV1a32("test");

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void FNV1a32_UsesDefaultSeed_WhenNotProvided()
        {
            uint hash = Helpers.FNV1a32("test");

            Assert.AreNotEqual(0u, hash);
        }

        [Test]
        public void FNV1a32_CanChainHashes()
        {
            uint hash1 = Helpers.FNV1a32("part1");
            uint hash2 = Helpers.FNV1a32("part2", hash1);

            // Hash should be different from individual parts
            Assert.AreNotEqual(hash1, hash2);
            Assert.AreNotEqual(Helpers.FNV1a32("part2"), hash2);
        }

        [Test]
        public void FNV1a32_ChainedHash_IsDifferent_ForDifferentOrder()
        {
            uint hash1 = Helpers.FNV1a32("part2", Helpers.FNV1a32("part1"));
            uint hash2 = Helpers.FNV1a32("part1", Helpers.FNV1a32("part2"));

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void FNV1a32_HandlesEmptyString()
        {
            uint hash = Helpers.FNV1a32("");

            // Should return the default seed
            Assert.AreEqual(2166136261u, hash);
        }

        [Test]
        public void GetElementFromHash_ReturnsElement_FromCollection()
        {
            string[] collection = { "A", "B", "C", "D", "E" };
            
            string result = Helpers.GetElementFromHash(collection, "test");

            Assert.IsTrue(System.Array.IndexOf(collection, result) >= 0);
        }

        [Test]
        public void GetElementFromHash_ReturnsSameElement_ForSameString()
        {
            string[] collection = { "A", "B", "C", "D", "E" };
            
            string result1 = Helpers.GetElementFromHash(collection, "test");
            string result2 = Helpers.GetElementFromHash(collection, "test");

            Assert.AreEqual(result1, result2);
        }

        [Test]
        public void GetElementFromHash_WorksWithMultipleElements()
        {
            string[] collection = { "A", "B", "C", "D", "E" };
            
            // Test that it returns valid elements from the collection
            for (int i = 0; i < 10; i++)
            {
                string result = Helpers.GetElementFromHash(collection, $"test{i}");
                Assert.IsTrue(System.Array.IndexOf(collection, result) >= 0);
            }
        }

        [Test]
        public void GetElementFromHash_WorksWithSingleElementCollection()
        {
            string[] collection = { "Only" };
            
            string result = Helpers.GetElementFromHash(collection, "test");

            Assert.AreEqual("Only", result);
        }
    }
}
