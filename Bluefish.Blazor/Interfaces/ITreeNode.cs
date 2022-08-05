namespace Bluefish.Blazor.Interfaces;

public interface ITreeNode
{
    /// <summary>
    /// Gets or sets the nodes CSS classes.
    /// </summary>
    string CssClass { get; set; }

    /// <summary>
    /// Gets the child nodes that are parented by this node.
    /// </summary>
    List<ITreeNode> ChildNodes { get; }

    /// <summary>
    /// Does this node contain child nodes?
    /// </summary>
    /// <remarks>A null value indicates unknown and that it will only be answered by
    /// attempting to fetch data (usually on an expand attempt).</remarks>
    bool? HasChildNodes { get; set; }

    /// <summary>
    /// Gets the nodes icon.
    /// </summary>
    string IconCssClass { get; }

    /// <summary>
    /// Gets or sets whether the node is expanded.
    /// </summary>
    bool IsExpanded { get; set; }

    /// <summary>
    /// Gets or sets whether the node can be selected.
    /// </summary>
    bool IsSelectable { get; set; }

    /// <summary>
    /// Gets the nodes key value.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Gets a stack containing this node and all parent nodes.
    /// </summary>
    Stack<ITreeNode> GetNodeStack();

    /// <summary>
    /// Gets the full path to this node.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets an array of all the nodes siblings.
    /// </summary>
    /// <param name="includeSelf">Should the node isteld be included in the result?</param>
    ITreeNode[] GetSiblings(bool includeSelf);

    /// <summary>
    /// Gets the parent node of this node.
    /// </summary>
    /// <remarks>A null value indicates the root node of a tree structure.</remarks>
    ITreeNode Parent { get; }

    /// <summary>
    /// Gets or sets the nodes optional state.
    /// </summary>
    object State { get; set; }

    /// <summary>
    /// Gets the nodes text.
    /// </summary>
    string Text { get; }
}