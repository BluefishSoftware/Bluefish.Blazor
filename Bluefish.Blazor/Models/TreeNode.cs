namespace Bluefish.Blazor.Models;

public class TreeNode<TItem> : ITreeNode<TItem>
{
    private List<ITreeNode<TItem>> _childNodes = new();

    /// <summary>
    /// Initializes a new TreeNode instance.
    /// </summary>
    /// <param name="item">Data for this node.</param>
    public TreeNode(TItem item)
    {
        Item = item;
    }

    /// <summary>
    /// Attempts to add a new child node for the given item.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    /// <returns>The newly created node.</returns>
    public ITreeNode<TItem> AddNode(TItem item)
    {
        // TODO: check key is unique in collection
        var newNode = new TreeNode<TItem>(item)
        {
            Parent = this
        };
        _childNodes.Add(newNode);
        return newNode;
    }

    /// <summary>
    /// Gets the child nodes that are parented by this node.
    /// </summary>
    public ITreeNode<TItem>[] ChildNodes => _childNodes.ToArray();

    /// <summary>
    /// Gets a function that returns the Key for the nodes data item.
    /// </summary>
    /// <remarks>The Key property is used to build a full path for any given node.</remarks>
    public Func<TItem, string> GetKey => (item) => item.ToString();

    /// <summary>
    /// Gets a stack containing this node and all parent nodes.
    /// </summary>
    public Stack<ITreeNode<TItem>> GetNodeStack()
    {
        Stack<ITreeNode<TItem>> stack = new();
        ITreeNode<TItem> n = this;
        while (n != null)
        {
            stack.Push(n);
            n = n.Parent;
        }
        return stack;
    }

    /// <summary>
    /// Gets the full path to this node.
    /// </summary>
    public string GetPath()
    {
        return "/" + String.Join("/", GetNodeStack().Select(x => x.GetKey(x.Item)));
    }

    /// <summary>
    /// Does this node contain child nodes?
    /// </summary>
    /// <remarks>A null value indicates unknown and that it will only be answered by
    /// attempting to fetch data (usually on an expand attempt).</remarks>
    public bool? HasChildNodes { get; set; }

    /// <summary>
    /// Gets the item for this node.
    /// </summary>
    public TItem Item { get; private set; }

    /// <summary>
    /// Gets the identifier for this node, must be unique between siblings.
    /// </summary>
    /// <remarks>The Key property is used to build a full path for any given node.</remarks>
    public string Key { get; set; }

    /// <summary>
    /// Gets the parent node of this node.
    /// </summary>
    /// <remarks>A null value indicates the root node of a tree structure.</remarks>
    public ITreeNode<TItem> Parent { get; set; }

    /// <summary>
    /// Gets an array of all the nodes siblings.
    /// </summary>
    /// <param name="includeSelf">Should the node isteld be included in the result?</param>
    public ITreeNode<TItem>[] GetSiblings(bool includeSelf)
    {
        if(Parent is null)
        {
            return includeSelf ? new[] { this } : Array.Empty<ITreeNode<TItem>>();
        }
        return includeSelf ? Parent.ChildNodes.ToArray() : Parent.ChildNodes.Except(new[] { this }).ToArray();
    }

    public override string ToString()
    {
        return GetPath();
    }
}
