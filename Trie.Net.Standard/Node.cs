using System.Collections.Generic;

namespace Trie.Net.Standard
{
    /// <summary>
    ///     The class for implementing a node in trie.
    ///     Trie is a rooted tree, each of its node has a corresponding value, a set of linked children and its linked parent,
    ///     as well as a boolean field to specify whether the node corresponds to the end of the key (target sequence of
    ///     values), or just a key prefix.
    /// </summary>
    /// <typeparam name="T">The value type of a node.</typeparam>
    public class Node<T>
    {
        public Node(T value, Node<T> parent = null)
        {
            Parent = parent;
            Value = value;
        }

        /// <summary>
        ///     A set of linked children of a node.
        /// </summary>
        public HashSet<Node<T>> Children { get; } = new HashSet<Node<T>>();

        /// <summary>
        ///     <code>true</code> if a node corresponds to the end of the searched key, otherwise <code>false</code>.
        /// </summary>
        public bool IsEnd { get; internal set; }

        /// <summary>
        ///     The linked parent of a node.
        /// </summary>
        public Node<T> Parent { get; }

        /// <summary>
        ///     The corresponding value of a node.
        /// </summary>
        public T Value { get; }
    }
}
