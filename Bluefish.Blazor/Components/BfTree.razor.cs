﻿namespace Bluefish.Blazor.Components;

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

    public async Task OnNodeClickAsync(ITreeNode node)
    {
        SelectedNode = node;
        await SelectedNodeChanged.InvokeAsync(node).ConfigureAwait(true);
        StateHasChanged();
    }

    public async Task OnNodeRefreshAsync(ITreeNode node)
    {
        await Task.Delay(100);
    }

}