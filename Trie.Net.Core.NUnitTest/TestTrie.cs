﻿using System;
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

        public static IEnumerable TestCaseContains
        {
            get
            {
                yield return new TestCaseData(new Predicate<Node<char>>(node => node.Value == 'e')).Returns(false);
                yield return new TestCaseData(new Predicate<Node<char>>(node => node.Value == 'o')).Returns(true);
            }
        }

        public static IEnumerable TestCaseInsert
        {
            get
            {
                yield return new TestCaseData(Presets,
                        new Predicate<IEnumerable<string>>(words => words.All(word => Presets.Contains(word))))
                    .Returns(true);
            }
        }

        public static IEnumerable TestCasePathTo
        {
            get
            {
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'o'),
                        new Predicate<IEnumerable<IEnumerable<Node<char>>>>(paths =>
                            paths.Select(path => new string(path.Select(node => node.Value).ToArray()))
                                .All(word => new[] {"Micro", "Microso"}.Contains(word))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 't'),
                        new Predicate<IEnumerable<IEnumerable<Node<char>>>>(paths =>
                            paths.Select(path => new string(path.Select(node => node.Value).ToArray()))
                                .All(word => new[] {"Microsoft", "MyScript"}.Contains(word))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'p'),
                        new Predicate<IEnumerable<IEnumerable<Node<char>>>>(paths =>
                            paths.Select(path => new string(path.Select(node => node.Value).ToArray()))
                                .All(word => new[] {"MyScrip"}.Contains(word))))
                    .Returns(true);
                yield return new TestCaseData(
                        new Predicate<Node<char>>(node => node.Value == 'y'),
                        new Predicate<IEnumerable<IEnumerable<Node<char>>>>(paths =>
                            paths.Select(path => new string(path.Select(node => node.Value).ToArray()))
                                .All(word => new[] {"My"}.Contains(word))))
                    .Returns(true);
            }
        }

        public static IEnumerable TestCaseRemove
        {
            get
            {
                yield return new TestCaseData(Presets,
                        new Predicate<IEnumerable<string>>(words => words.All(word => !Presets.Contains(word))))
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
        [TestCase("Microphone", "Microwave", ExpectedResult = false)]
        public bool TestContains(params string[] words)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            return words.All(word => Trie.Contains(word.ToCharArray()));
        }

        [Test]
        [TestCaseSource(nameof(TestCaseContains))]
        public bool TestContains(Predicate<Node<char>> predicate)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            return Trie.Contains(predicate);
        }

        [Test]
        [TestCaseSource(nameof(TestCaseInsert))]
        public bool TestInsert(IEnumerable<string> words, Predicate<IEnumerable<string>> expected)
        {
            foreach (var word in words) Trie.Insert(word.ToCharArray());
            return expected(Trie.Keys.Select(key => new string(key.ToArray())));
        }

        [Test]
        [TestCaseSource(nameof(TestCasePathTo))]
        public bool TestPathTo(Predicate<Node<char>> predicate,
            Predicate<IEnumerable<IEnumerable<Node<char>>>> expected)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            return expected(Trie.PathTo(predicate));
        }

        [Test]
        [TestCase("Micro", "Microsoft", ExpectedResult = "Micro")]
        [TestCase("Micro", "Microsoft", "MyScript", ExpectedResult = "M")]
        public string TestPrefix(params string[] words)
        {
            foreach (var word in words) Trie.Insert(word.ToCharArray());
            return new string(Trie.Prefix.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(TestCaseRemove))]
        public bool TestRemove(IEnumerable<string> words, Predicate<IEnumerable<string>> expected)
        {
            foreach (var preset in Presets) Trie.Insert(preset.ToCharArray());
            foreach (var word in words) Trie.Remove(word.ToCharArray());
            return expected(Trie.Keys.Select(key => new string(key.ToArray())));
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
