namespace Bluefish.Blazor.Components;

public partial class BfTreeNode
{
    [CascadingParameter]
    public BfTree Tree { get; set; }

    [Parameter]
    public ITreeNode Node { get; set; }

    private string GetCssClass()
    {
        var sb = new StringBuilder();
        if (Node.IsSelectable)
        {
            sb.Append("selectable ");
        }
        if (Tree != null && Tree.SelectedNode == Node)
        {
            sb.Append("selected ");
        }
        sb.Append(Node.CssClass);
        return sb.ToString();
    }

    private async Task OnToggleExpandAsync()
    {
        if (Node.HasChildNodes == null)
        {
            // initial refresh of node
            await Tree.OnNodeRefreshAsync(Node).ConfigureAwait(true);
            Node.HasChildNodes = Node.ChildNodes.Count > 0;
            Node.IsExpanded = true;
        }
        else
        {
            Node.IsExpanded = !Node.IsExpanded;
        }
    }

    private async Task OnClickAsync()
    {
        if (Tree != null)
        {
            await Tree.OnNodeClickAsync(Node).ConfigureAwait(true);
        }
    }
}
