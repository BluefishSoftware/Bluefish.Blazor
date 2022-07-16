using Bluefish.Blazor.Utility;
using System.Reflection;

namespace Bluefish.Blazor.Components;

public partial class BfTableToolbar<TItem, TKey> : IAsyncDisposable
{
    private BfTable<TItem, TKey> _table;
    private IJSObjectReference _commonModule;
    private BfDropDown _filterDropdown = null!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public RenderFragment LeftContent { get; set; }

    [Parameter]
    public RenderFragment RightContent { get; set; }

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_commonModule != null)
            {
                await _commonModule.DisposeAsync().ConfigureAwait(true);
            }
        }
        catch
        {
        }
    }

    private string GetFilterName(Filter filter)
    {
        var col = _table?.AllColumns.FirstOrDefault(x => x.FilterKey == filter.Key);
        if (col != null)
        {
            return col.HeaderText;
        }
        return String.Empty;
    }

    private Type GetFilterType(Filter filter)
    {
        var col = _table?.AllColumns.FirstOrDefault(x => x.FilterKey == filter.Key);
        if (col != null)
        {
            if (col.DataType != null)
            {
                return col.DataType;
            }
            var members = PropertyPath<TItem>.Get(col.DataMember);
            return members.LastOrDefault() is PropertyInfo pi ? pi.PropertyType : typeof(String);
        }
        return typeof(String);
    }

    private string[] GetFilterKeys()
    {
        if (_table is null)
        {
            return Array.Empty<string>();
        }
        return _table.AllColumns
            .Where(x => !string.IsNullOrWhiteSpace(x.FilterKey))
            .OrderBy(x => x.HeaderText)
            .Select(x => x.FilterKey)
            .ToArray();
    }

    public void SetTable(BfTable<TItem, TKey> table)
    {
        _table = table;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _commonModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/js/common.js").ConfigureAwait(true);
        }
    }

    private void OnFilterChanged(string searchText)
    {
        if (_table?.FilterInfo != null)
        {
            _table.FilterInfo.SearchText = searchText;
        }
    }

    private async Task OnFilterPanelApply()
    {
        await _filterDropdown.HideAsync().ConfigureAwait(true);
        await _table.RefreshAsync().ConfigureAwait(true);
    }


    private async Task OnFilterPanelClose()
    {
        await _filterDropdown.HideAsync().ConfigureAwait(true);
    }

    private async Task OnRefreshAsync()
    {
        if (_table != null)
        {
            await _table.RefreshAsync().ConfigureAwait(true);
        }
    }

    private void OnSearchTextChanged(ChangeEventArgs args)
    {
        if (_table?.FilterInfo != null)
        {
            _table.FilterInfo.SearchText = args.Value?.ToString() ?? String.Empty;
        }
    }

    private async Task OnSearchTextKeyPress(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" && _table != null)
        {
            var searchText = await _commonModule.InvokeAsync<string>("getValue", $"{Id}-searchbox").ConfigureAwait(true);
            _table.FilterInfo.SearchText = searchText;
            await OnRefreshAsync().ConfigureAwait(true);
        }
    }
}
