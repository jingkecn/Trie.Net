using System;
using System.Collections.Generic;
using System.Linq;

namespace Trie.Net.Standard
{
    /// <summary>
    ///     The class for implementing a trie.
    ///     Trie, aka. prefix tree, by its
    ///     <see href="https://leetcode.com/articles/implement-trie-prefix-tree/">definition</see>, is a tree data structure,
    ///     which is used for retrieval of a key in a data set.
    /// </summary>
    /// <typeparam name="T">The value type of a trie.</typeparam>
    public partial class Trie<T>
    {
        /// <summary>
        ///     Keys of a trie.
        /// </summary>
        public IEnumerable<IEnumerable<T>> Keys =>
            PathTo(node => node.IsEnd).Select(nodes => nodes.Select(node => node.Value));

        /// <summary>
        ///     Longest common prefix among all keys of a trie.
        /// </summary>
        public IEnumerable<T> Prefix
        {
            get
            {
                var node = Root;
                while (node.Children.Count() == 1)
                {
                    node = node.Children.Single();
                    yield return node.Value;
                    if (node.IsEnd) yield break;
                }
            }
        }

        /// <summary>
        ///     The root node of a trie.
        /// </summary>
        public Node<T> Root { get; } = new Node<T>(default);

        /// <summary>
        ///     Existence of predicated nodes.
        /// </summary>
        /// <param name="predicate">Predicated criteria.</param>
        /// <returns><code>true</code> if the trie contains the predicated nodes, otherwise <code>false</code>.</returns>
        public bool Contains(Predicate<Node<T>> predicate)
        {
            return Search(predicate).Count() != 0;
        }

        /// <summary>
        ///     Existence of a key.
        /// </summary>
        /// <param name="values">The key, in form of a <code>T</code> value sequence.</param>
        /// <returns><code>true</code> if the key is in the trie, otherwise <code>false</code>.</returns>
        public bool Contains(params T[] values)
        {
            return Keys.Any(key => key.SequenceEqual(values));
        }

        /// <summary>
        ///     Insertion of a key to a trie.
        ///     We insert a key by searching into a trie. We start from the root and search a linked child, which corresponds to
        ///     the first key value. There are two cases:
        ///     - A linked child exists. Then we move down the tree following the linked children to the next child level. The
        ///     algorithm continues with searching for the next key value.
        ///     - A linked child does not exists. Then we create a new node and link it with the parent's link matching the current
        ///     key value. We repeat this step until we encounter the last value of the key, then we mark the current node as an
        ///     end node and the algorithm finishes.
        /// </summary>
        /// <param name="values">The key to insert, in form of a sequence of <code>T</code>-typed values.</param>
        public void Insert(params T[] values)
        {
            var node = Root;
            foreach (var value in values)
            {
                if (node.Children.All(child => !child.Value.Equals(value)))
                    (node.Children as HashSet<Node<T>>)?.Add(new Node<T>(value, node));
                node = node.Children.Single(child => child.Value.Equals(value));
            }

            node.IsEnd = true;
        }

        /// <summary>
        ///     Paths from root to predicated nodes.
        /// </summary>
        /// <param name="predicate">Predicated criteria.</param>
        /// <returns>Paths from root to the predicated nodes.</returns>
        public IEnumerable<IEnumerable<Node<T>>> PathTo(Predicate<Node<T>> predicate)
        {
            foreach (var result in Search(predicate))
            {
                var node = result;
                var stack = new Stack<Node<T>>();
                do
                {
                    stack.Push(node);
                    node = node.Parent;
                } while (node != Root);

                yield return stack.ToArray();
            }
        }

        /// <summary>
        ///     Removal of a key from a trie.
        ///     We remove a key by searching into a trie. We start form the end node, which corresponds to the last value of the
        ///     key. There are two cases:
        ///     - The end node is a shared node. This is to say, there must be at least one other key that is prefixed by the key
        ///     to remove, such as the strings "Microsoft" and "Micro". Then we just remove the end mark of the current node.
        ///     - The end node is not a shared node. Then we search the nearest shared parent by moving up the tree following the
        ///     linked parent to the last parent level until the parent of the node has more than one child, then we remove the
        ///     node from its linked parent and the algorithm finishes.
        /// </summary>
        /// <param name="values">The key to remove, in form of a sequence of <code>T</code>-typed values.</param>
        public void Remove(params T[] values)
        {
            var node = Root;
            foreach (var value in values)
                if (node.Children.Any(child => child.Value.Equals(value)))
                    node = node.Children.Single(child => child.Value.Equals(value));
                else return;
            if (node.Children.Count() != 0)
            {
                node.IsEnd = false;
                return;
            }

            var parent = node.Parent;
            while (parent != Root && parent.Children.Count() == 1)
            {
                node = parent;
                parent = node.Parent;
            }

            (parent.Children as HashSet<Node<T>>)?.Remove(node);
        }

        /// <summary>
        ///     Search nodes that satisfy the predicated criteria.
        ///     We search by
        ///     <see href="https://en.wikipedia.org/w/index.php?title=Depth-first_search&amp;oldid=896938638">DFS</see> algorithm.
        ///     We start from the root and search a linked child, there are two cases:
        ///     - The child satisfies the prediction. Then we yield and return the current node.
        ///     - The child does not satisfy the prediction. Then we continue to search its children and repeat the previous step
        ///     until all nodes are checked.
        /// </summary>
        /// <param name="predicate">Predicated criteria.</param>
        /// <returns>Nodes satisfying the predicated criteria.</returns>
        public IEnumerable<Node<T>> Search(Predicate<Node<T>> predicate)
        {
            return Search(Root, predicate).Distinct();
        }
    }

    public partial class Trie<T>
    {
        private static IEnumerable<Node<T>> Search(Node<T> root, Predicate<Node<T>> predicate)
        {
            if (predicate(root)) yield return root;
            foreach (var child in root.Children)
            foreach (var node in Search(child, predicate))
                yield return node;
        }
    }
}
