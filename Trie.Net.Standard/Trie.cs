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
    public class Trie<T>
    {
        /// <summary>
        ///     The latest common node of all keys of a tries.
        ///     It corresponds to the last key value of the longest common prefix of all keys.
        /// </summary>
        public Node<T> LatestCommonNode
        {
            get
            {
                var node = Root;
                while (node.Children.Count == 1)
                    node = node.Children.Single();
                return node;
            }
        }

        /// <summary>
        ///     The root node of a trie.
        /// </summary>
        public Node<T> Root { get; } = new Node<T>(default);

        /// <summary>
        ///     Existence of a key in a trie.
        ///     A key exists if and only if a trie has a complete path for the sequence of key values and the node corresponding to
        ///     the last key value is an end node. For example, within a trie containing the string "Microsoft", both "Microwave"
        ///     and "Micro" do not exist.
        /// </summary>
        /// <param name="values">The key to verify, in form of a sequence of <code>T</code>-typed values.</param>
        /// <returns></returns>
        public bool Exists(params T[] values)
        {
            var node = Root;
            foreach (var value in values)
                if (node.Children.Any(child => child.Value.Equals(value)))
                    node = node.Children.Single(child => child.Value.Equals(value));
                else return false;
            return node.IsEnd;
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
                    node.Children.Add(new Node<T>(value, node));
                node = node.Children.Single(child => child.Value.Equals(value));
            }

            node.IsEnd = true;
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
            if (node.Children.Count != 0)
            {
                node.IsEnd = false;
                return;
            }

            var parent = node.Parent;
            while (parent != Root && parent.Children.Count == 1)
            {
                node = parent;
                parent = node.Parent;
            }

            parent.Children.Remove(node);
        }
    }
}
