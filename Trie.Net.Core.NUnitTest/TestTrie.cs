using System.Linq;
using NUnit.Framework;
using Trie.Net.Standard;

namespace Trie.Net.Core.NUnitTest
{
    [TestFixture]
    public class TestTrie
    {
        [SetUp]
        public void SetUp()
        {
            Trie = new Trie<char>();
        }

        private Trie<char> Trie { get; set; }

        [Test]
        [TestCase("Micro", "Microsoft", "MyScript")]
        public void TestExists(params string[] words)
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            Assert.IsTrue(words.All(word => Trie.Exists(word.ToCharArray())));
        }

        [Test]
        [TestCase("Micro", "Microsoft", "MyScript")]
        public void TestInsert(params string[] words)
        {
            foreach (var word in words) Trie.Insert(word.ToCharArray());
            Assert.IsTrue(words.All(word => Trie.Exists(word.ToCharArray())));
        }

        [Test]
        [TestCase("Microbe", "Microphone")]
        public void TestNotExists(params string[] words)
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            Assert.IsFalse(words.Any(word => Trie.Exists(word.ToCharArray())));
        }
    }
}
