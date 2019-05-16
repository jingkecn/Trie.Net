using System.Collections.Generic;

namespace Trie.Net.Standard
{
    public class Node<T>
    {
        public Node(T value, Node<T> parent = null)
        {
            Parent = parent;
            Value = value;
        }

        public HashSet<Node<T>> Children { get; } = new HashSet<Node<T>>();
        public bool IsEnd { get; internal set; }
        public Node<T> Parent { get; }
        public T Value { get; }
    }
}
