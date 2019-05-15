using System.Linq;

namespace Trie.Net.Standard
{
    public class Trie<T>
    {
        private Node<T> Root { get; } = new Node<T>(default);

        public Node<T> LatestCommonNode
        {
            get
            {
                var node = Root;
                while (node.Children.Count == 1)
                    node = node.Children.Single();
                return node == Root ? null : node;
            }
        }

        public bool Exists(params T[] values)
        {
            var node = Root;
            foreach (var value in values)
                if (node.Children.Any(child => child.Value.Equals(value)))
                    node = node.Children.Single(child => child.Value.Equals(value));
                else return false;
            return true;
        }

        public void Insert(params T[] values)
        {
            var node = Root;
            foreach (var value in values)
            {
                if (node.Children.All(child => !child.Value.Equals(value)))
                    node.Children.Add(new Node<T>(value, node == Root ? null : node));
                node = node.Children.Single(child => child.Value.Equals(value));
            }
        }
    }
}
