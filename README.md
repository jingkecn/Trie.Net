# Trie.Net

[![NuGet version](https://badge.fury.io/nu/Trie.Net.Standard.svg)](https://badge.fury.io/nu/Trie.Net.Standard)

> A small, lean and generic implementation of a [Trie](https://en.wikipedia.org/w/index.php?title=Trie&oldid=897578302) for .Net.

A brief description of your project, what it is used for and how does life get
awesome when someone starts to use it.

**Trie.Net** is, by definition from its name, a .Net library of the very classic data structure, Trie[[1]](#links). It is inspired from a LeetCode article[[2]](#links).

- [Trie.Net](#trienet)
  - [Installing / Getting started](#installing--getting-started)
  - [Features](#features)
    - [Insert A Key](#insert-a-key)
    - [Remove A Key](#remove-a-key)
    - [Others](#others)
      - [Properties](#properties)
      - [Methods](#methods)
  - [Contributing](#contributing)
  - [Links](#links)
  - [Licensing](#licensing)

## Installing / Getting started

You can find the installing instructions on [NuGet](https://www.nuget.org/packages/Trie.Net.Standard/).

Once the packages are installed from NuGet, you can start from a first try:

```csharp
// 1 - Instantiate a word tree.
var trie = new Trie<char>();
// 2 - Add string "Hello world!".
trie.Insert("Microsoft".ToCharArray());
// 3 - Add string "Hello guys!".
trie.Insert("Microbe".ToCharArray());
// 4 - Print the longest common prefix.
Console.WriteLine(trie.LongestCommonPrefix);

// The console output should be:
// Micro
```

> You think that's it?! No, it is a lot more than you could imagine! Try me!

## Features

This project is mainly to offer a small and lean set of APIs for a `Trie`, with the following functionalities:

### Insert A Key

You may have already seen this API from the example in the previous section, it is quite easy to insert a key, which is a `string` in the example, it is supposed to be a chain of `T`-typed values, which is a chain of `char` values in the example:

```csharp
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
Trie.Insert(params T[] values);
```

### Remove A Key

It is as easy as key insertion to remove a key, via the API `Trie.Remove(params T[] values)`:

```csharp
/// <summary>
///     Removal of a key from a trie.
///     We remove a key by searching into a trie. We start from the end node, which corresponds to the last value of the
///     key. There are two cases:
///     - The end node is a shared node. This is to say, there must be at least one other key that is prefixed by the key
///     to remove, such as the strings "Microsoft" and "Micro". Then we just remove the end mark of the current node.
///     - The end node is not a shared node. Then we search the nearest shared parent by moving up the tree following the
///     linked parent to the last parent level until the parent of the node has more than one child, then we remove the
///     node from its linked parent and the algorithm finishes.
/// </summary>
/// <param name="values">The key to remove, in form of a sequence of <code>T</code>-typed values.</param>
Trie.Remove(params T[] values);
```

### Others

In addition to the two main functionalities above, there are also other APIs that could help us develop with a `Trie`:

#### Properties

| Name                       | Type              | Description                                                                                         |
| -------------------------- | ----------------- | --------------------------------------------------------------------------------------------------- |
| `Trie.Keys`                | `IEnumeration<T>` | Keys in the `Trie`, each of whose last value corresponds to an end node.                            |
| `Trie.LongestCommonPrefix` | `IEnumeration<T>` | Longest common prefix of all keys.                                                                  |
| `Trie.Root`                | `Node<T>`         | The `Root` node holds all branches of the `Trie`, with a `default` value depending on the type `T`. |

#### Methods

| Name                                        | Return Type                         | Description                                                                                                                                                                                                                       |
| ------------------------------------------- | ----------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Trie.Contains(params T[] values)`          | `bool`                              | Check the existence of a key.                                                                                                                                                                                                     |
| `Trie.PathTo(Predicate<Node<T>> predicate)` | `IEnumerable<IEnumerable<Node<T>>>` | Returns a list of path from the `Root` to a predicable node. The parameter `predicate` is a [`Predicate<Node<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1) defining the criteria to predicate a `Node<T>`. |
| `Trie.Search(Predicate<Node<T>> predicate)` | `IEnumerable<Node<T>>`              | Returns a list of node that satisfies the criteria of predicable node.                                                                                                                                                            |

## Contributing

Better ideas or improvements are welcome if you have any, just [fork](https://github.com/jingkecn/Trie.Net) me and send a **pull request** and let's see what could be going on.

## Links

1. [Trie | Wikipedia](https://en.wikipedia.org/w/index.php?title=Trie&oldid=897578302)
2. [Implement Trie (Prefix Tree) | LeetCode](https://leetcode.com/articles/implement-trie-prefix-tree/)

## Licensing

The code in this project is [licensed](LICENSE) under MIT license.
