namespace Bluefish.Blazor.Interfaces;

public interface ITreeNode<TItem>
{
    /// <summary>
    /// Attempts to add a new child node for the given item.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    /// <returns>The newly created node.</returns>
    ITreeNode<TItem> AddNode(TItem item);

    /// <summary>
    /// Gets the child nodes that are parented by this node.
    /// </summary>
    ITreeNode<TItem>[] ChildNodes { get; }

    /// <summary>
    /// Does this node contain child nodes?
    /// </summary>
    /// <remarks>A null value indicates unknown and that it will only be answered by
    /// attempting to fetch data (usually on an expand attempt).</remarks>
    bool? HasChildNodes { get; }

    /// <summary>
    /// Gets a function that returns the Key for the nodes data item.
    /// </summary>
    /// <remarks>The Key property is used to build a full path for any given node.</remarks>
    Func<TItem, string> GetKey { get; }

    /// <summary>
    /// Gets a stack containing this node and all parent nodes.
    /// </summary>
    Stack<ITreeNode<TItem>> GetNodeStack();

    /// <summary>
    /// Gets the full path to this node.
    /// </summary>
    string GetPath();

    /// <summary>
    /// Gets the parent node of this node.
    /// </summary>
    /// <remarks>A null value indicates the root node of a tree structure.</remarks>
    ITreeNode<TItem> Parent { get; }

    /// <summary>
    /// Gets an array of all the nodes siblings.
    /// </summary>
    /// <param name="includeSelf">Should the node isteld be included in the result?</param>
    ITreeNode<TItem>[] GetSiblings(bool includeSelf);

    /// <summary>
    /// Gets the item at this node.
    /// </summary>
    TItem Item { get; }
}