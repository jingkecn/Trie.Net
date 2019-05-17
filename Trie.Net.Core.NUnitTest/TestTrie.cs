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
        [TestCase("Micro", "Microsoft", "MyScript", ExpectedResult = true)]
        [TestCase("Microbe", "Microphone", ExpectedResult = false)]
        public bool TestExists(params string[] words)
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            return words.All(word => Trie.Exists(word.ToCharArray()));
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
        [TestCase('f', ExpectedResult = "Microsof")]
        [TestCase('p', ExpectedResult = "MyScrip")]
        public string TestPathTo(char target)
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            return new string(Trie.PathTo(node => node.Value == target).Select(item => item.Value).ToArray());
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

        [Test]
        [TestCase('o')]
        public void TestSearch(char target)
        {
            Trie.Insert("Micro".ToCharArray());
            Trie.Insert("Microsoft".ToCharArray());
            Trie.Insert("MyScript".ToCharArray());
            var results = Trie.Search(node => node.Value == target).ToList();
            Assert.IsNotEmpty(results);
            Assert.IsTrue(results.All(result =>
                result.Value == target &&
                "rs".Contains(result.Parent.Value) /* the parent of 'o' is either 'r' or 's' */ &&
                result.Children.All(child => "sf".Contains(child.Value) /* the child of 'o' is either 's' or 'f' */)));
        }
    }
}
