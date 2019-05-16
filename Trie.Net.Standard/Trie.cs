using System.Linq;

namespace Trie.Net.Standard
{
    public class Trie<T>
    {
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

        private Node<T> Root { get; } = new Node<T>(default);

        public bool Exists(params T[] values)
        {
            var node = Root;
            foreach (var value in values)
                if (node.Children.Any(child => child.Value.Equals(value)))
                    node = node.Children.Single(child => child.Value.Equals(value));
                else return false;
            return node.IsEnd;
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

            node.IsEnd = true;
        }

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

            var parent = node.Parent ?? Root;
            while (parent != Root && parent.Children.Count == 1)
            {
                node = parent;
                parent = node.Parent ?? Root;
            }

            parent.Children.Remove(node);
        }
    }
}
