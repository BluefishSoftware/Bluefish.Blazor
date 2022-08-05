namespace Bluefish.Blazor.Models;

public class TreeNode : ITreeNode
{
    /// <summary>
    /// Initializes a new TreeNode instance.
    /// </summary>
    public TreeNode()
    {
    }

    /// <summary>
    /// Gets the child nodes that are parented by this node.
    /// </summary>
    public List<ITreeNode> ChildNodes { get; } = new();

    /// <summary>
    /// Gets or sets the nodes CSS classes.
    /// </summary>
    public string CssClass { get; set; } = String.Empty;

    /// <summary>
    /// Gets a function that returns the Key for the nodes data item.
    /// </summary>
    /// <remarks>The Key property is used to build a full path for any given node.</remarks>
    //public Func<TItem, string> GetKey => (item) => item.ToString();

    /// <summary>
    /// Gets a stack containing this node and all parent nodes.
    /// </summary>
    public Stack<ITreeNode> GetNodeStack()
    {
        Stack<ITreeNode> stack = new();
        ITreeNode n = this;
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
    public string Path => "/" + String.Join("/", GetNodeStack().Select(x => x.Key));

    /// <summary>
    /// Does this node contain child nodes?
    /// </summary>
    /// <remarks>A null value indicates unknown and that it will only be answered by
    /// attempting to fetch data (usually on an expand attempt).</remarks>
    public bool? HasChildNodes { get; set; }

    /// <summary>
    /// Gets or sets the nodes icon.
    /// </summary>
    public string IconCssClass { get; set; }

    /// <summary>
    /// Gets or sets whether the node is expanded.
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// Gets or sets whether the node can be selected.
    /// </summary>
    public bool IsSelectable { get; set; } = true;

    /// <summary>
    /// Gets the nodes key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets the parent node of this node.
    /// </summary>
    /// <remarks>A null value indicates the root node of a tree structure.</remarks>
    public ITreeNode Parent { get; set; }

    /// <summary>
    /// Gets an array of all the nodes siblings.
    /// </summary>
    /// <param name="includeSelf">Should the node isteld be included in the result?</param>
    public ITreeNode[] GetSiblings(bool includeSelf)
    {
        if (Parent is null)
        {
            return includeSelf ? new[] { this } : Array.Empty<ITreeNode>();
        }
        return includeSelf ? Parent.ChildNodes.ToArray() : Parent.ChildNodes.Except(new[] { this }).ToArray();
    }

    /// <summary>
    /// Gets the nodes optional state.
    /// </summary>
    public object State { get; set; }

    /// <summary>
    /// Gets the nodes text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    public override string ToString()
    {
        return Path;
    }
}
