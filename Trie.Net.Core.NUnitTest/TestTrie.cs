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
        public void TestLatestCommonNode()
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            var node = Trie.LatestCommonNode;
            var root = Trie.Root;
            Assert.AreEqual(root, node.Parent);
            Assert.AreEqual('M', node.Value);
            Assert.IsTrue(node.Children.All(child => child.Value == 'i' || child.Value == 'y'));
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

        [Test]
        [TestCase("Micro")]
        public void TestRemove(params string[] words)
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            foreach (var word in words)
            {
                Trie.Remove(word.ToCharArray());
                Assert.IsFalse(Trie.Exists(word.ToCharArray()));
            }

            Assert.IsTrue(Trie.Exists("Microsoft".ToCharArray()));
            Assert.IsTrue(Trie.Exists("MyScript".ToCharArray()));
        }

        [Test]
        public void TestRoot()
        {
            var root = Trie.Root;
            Assert.IsFalse(root.IsEnd);
            Assert.AreEqual(null, root.Parent);
            Assert.AreEqual(default(char), root.Value);
        }
    }
}
