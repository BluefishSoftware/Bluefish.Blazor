namespace Bluefish.Blazor.Components;

public partial class BfTree
{
    [Parameter]
    public ITreeNode Root { get; set; }

    [Parameter]
    public ITreeNode SelectedNode { get; set; }

    [Parameter]
    public EventCallback<ITreeNode> SelectedNodeChanged { get; set; }

    [Parameter]
    public bool ShowRoot { get; set; } = true;

    [Parameter]
    public bool SelectionToggle { get; set; }

    public async Task OnNodeClickAsync(ITreeNode node)
    {
        if (SelectionToggle && SelectedNode == node)
        {
            SelectedNode = null;
        }
        else
        {
            SelectedNode = node;
        }
        await SelectedNodeChanged.InvokeAsync(SelectedNode).ConfigureAwait(true);
        StateHasChanged();
    }

    public async Task OnNodeRefreshAsync(ITreeNode node)
    {
        await Task.Delay(100);
    }

}
