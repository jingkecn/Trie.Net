using System;
using System.Collections;
using System.Collections.Generic;
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

        private static IEnumerable<string> Presets => new[] {"Micro", "Microsoft", "MyScript"};

        public static IEnumerable TestCasePathTo
        {
            get
            {
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'o' && node.IsEnd),
                        new Predicate<IEnumerable<Node<char>>>(nodes =>
                            "Micro".SequenceEqual(nodes.Select(node => node.Value))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'p'),
                        new Predicate<IEnumerable<Node<char>>>(nodes =>
                            "MyScrip".SequenceEqual(nodes.Select(node => node.Value))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'y'),
                        new Predicate<IEnumerable<Node<char>>>(nodes =>
                            "My".SequenceEqual(nodes.Select(node => node.Value))))
                    .Returns(true);
            }
        }

        public static IEnumerable TestCaseSearch
        {
            get
            {
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'o' && node.IsEnd),
                        new Predicate<IEnumerable<Node<char>>>(nodes =>
                            new[] {'o'}.SequenceEqual(nodes.Select(node => node.Value))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'p'),
                        new Predicate<IEnumerable<Node<char>>>(nodes =>
                            new[] {'p'}.SequenceEqual(nodes.Select(node => node.Value))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'y'),
                        new Predicate<IEnumerable<Node<char>>>(nodes =>
                            new[] {'y'}.SequenceEqual(nodes.Select(node => node.Value))))
                    .Returns(true);
            }
        }

        private Trie<char> Trie { get; set; }

        [Test]
        [TestCase("Micro", "Microsoft", "MyScript", ExpectedResult = true)]
        [TestCase("Microbe", "Microphone", ExpectedResult = false)]
        public bool TestExists(params string[] words)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
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
        [TestCase("Micro", "Microsoft", ExpectedResult = "Micro")]
        [TestCase("Micro", "Microsoft", "MyScript", ExpectedResult = "M")]
        public string TestLongestCommonPrefix(params string[] words)
        {
            foreach (var word in words) Trie.Insert(word.ToCharArray());
            return new string(Trie.LongestCommonPrefix.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(TestCasePathTo))]
        public bool TestPathTo(Predicate<Node<char>> predicate, Predicate<IEnumerable<Node<char>>> expected)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            return expected(Trie.PathTo(predicate));
        }

        [Test]
        [TestCase("Micro", ExpectedResult = true)]
        public bool TestRemove(params string[] words)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            foreach (var word in words) Trie.Remove(word.ToCharArray());
            var remaining = Presets.Except(words);
            return words.All(word => !Trie.Exists(word.ToCharArray())) &&
                   remaining.All(word => Trie.Exists(word.ToCharArray()));
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
        [TestCaseSource(nameof(TestCaseSearch))]
        public bool TestSearch(Predicate<Node<char>> predicate, Predicate<IEnumerable<Node<char>>> expected)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            return expected(Trie.Search(predicate));
        }
    }
}
