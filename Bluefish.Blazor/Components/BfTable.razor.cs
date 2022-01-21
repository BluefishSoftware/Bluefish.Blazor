namespace Bluefish.Blazor.Components;

public partial class BfTable<TItem, TKey> : IDisposable
{
    private readonly List<BfColumn<TItem, TKey>> _columns = new();
    private readonly List<TKey> _selectedKeys = new();
    private IEnumerable<TItem> _dataItems;
    private bool _isLoading = true;

    [Parameter]
    public bool AllowSort { get; set; } = false;

    [Parameter]
    public bool AutoLoad { get; set; } = true;

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<TItem> RowClick { get; set; }

    [Parameter]
    public EventCallback<TItem> RowDoubleClick { get; set; }

    [Parameter]
    public Func<TItem, TKey> GetItemKey { get; set; } = (_) => default;

    [Parameter]
    public Func<PageInfo, SortInfo, Task<IEnumerable<TItem>>> GetPagedDataAsync { get; set; }

    [Parameter]
    public Func<IEnumerable<TKey>, Task<IEnumerable<TItem>>> GetSelectedDataAsync { get; set; }

    [Parameter]
    public RenderFragment NoDataTemplate { get; set; }

    [Parameter]
    public PageInfo PageInfo { get; set; }

    [Parameter]
    public EventCallback<IEnumerable<TKey>> SelectionChanged { get; set; }

    [Parameter]
    public SelectionModes SelectionMode { get; set; }

    [Parameter]
    public bool ShowCheckboxes { get; set; }

    [Parameter]
    public bool ShowHeader { get; set; } = true;

    [Parameter]
    public bool ShowPager { get; set; }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public SortInfo SortInfo { get; set; }

    [Parameter]
    public Func<TItem, string> TrCssClass { get; set; } = (_) => string.Empty;

    public BfColumn<TItem, TKey>[] VisibleColumns => _columns.Where(x => x.IsVisible).ToArray();

    public BfTable()
    {
        CssClass = "table table-striped";
    }

    private Dictionary<string, object> ActualAttributes
    {
        get
        {
            var attr = new Dictionary<string, object>(RootAttributes ?? new())
                {
                    { "class", CssClass }
                };
            if (!Visible)
            {
                attr.Add("style", "display: none;");
            }
            return attr;
        }
    }

    public void AddColumn(BfColumn<TItem, TKey> column)
    {
        _columns.Add(column);
        StateHasChanged();
    }

    public async Task ClearSelectionAsync()
    {
        _selectedKeys.Clear();
        await SelectionChanged.InvokeAsync(Array.Empty<TKey>()).ConfigureAwait(true);
    }

    public void Dispose()
    {
        if (PageInfo != null)
        {
            PageInfo.Changed -= PageInfo_Changed;
        }
        GC.SuppressFinalize(this);
    }

    public string GetSelectionCssClass(TItem item)
    {
        var sb = new StringBuilder();

        // change cursor if selectable
        if (SelectionMode != SelectionModes.None)
        {
            sb.Append("cursor-pointer ");
        }

        // when check boxes are shown for selection - do not highlight the row
        if (!(SelectionMode == SelectionModes.Multiple && ShowCheckboxes))
        {
            var key = GetItemKey(item);
            sb.Append(_selectedKeys.Contains(key) ? "selected " : string.Empty);
        }

        return sb.ToString().Trim();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (AutoLoad)
            {
                await RefreshAsync().ConfigureAwait(true);
            }
        }
    }

    private async Task OnItemCheckChange(TItem item, bool value)
    {
        var key = GetItemKey(item);
        if (value && !_selectedKeys.Contains(key))
        {
            _selectedKeys.Add(key);
            await SelectionChanged.InvokeAsync(_selectedKeys).ConfigureAwait(true);
        }
        else if (!value && _selectedKeys.Contains(key))
        {
            _selectedKeys.Remove(key);
            await SelectionChanged.InvokeAsync(_selectedKeys).ConfigureAwait(true);
        }
    }

    protected Task OnColumnHeaderClick(BfColumn<TItem, TKey> column)
    {
        if (AllowSort && column.IsSortable && SortInfo != null)
        {
            SortInfo.Direction = SortInfo.DataField == column.DataField
                ? (SortInfo.Direction == SortDirections.Descending ? SortDirections.Ascending : SortDirections.Descending)
                : SortDirections.Ascending;
            SortInfo.DataField = column.DataField;
            return RefreshAsync();
        }
        return Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        if (PageInfo != null)
        {
            PageInfo.Changed += PageInfo_Changed;
        }
    }

    private async Task OnRowClickAsync(MouseEventArgs args, TItem item)
    {
        await UpdateSelectionAsync(args, item).ConfigureAwait(true);
        await RowClick.InvokeAsync(item).ConfigureAwait(true);
    }

    private async Task OnRowDoubleClickAsync(MouseEventArgs args, TItem item)
    {
        await UpdateSelectionAsync(args, item).ConfigureAwait(true);
        await RowDoubleClick.InvokeAsync(item).ConfigureAwait(true);
    }

    private async void PageInfo_Changed(object sender, EventArgs e)
    {
        await InvokeAsync(RefreshAsync).ConfigureAwait(true);
    }

    public async Task RefreshAsync()
    {
        _isLoading = true;
        if (GetPagedDataAsync is null)
        {
            _dataItems = null;
        }
        else
        {
            _dataItems = await GetPagedDataAsync(PageInfo, SortInfo).ConfigureAwait(true);
        }
        _isLoading = false;
        StateHasChanged();
    }

    public IEnumerable<TKey> SelectionKeys
    {
        get
        {
            return _selectedKeys;
        }
    }

    public async Task SetSelectionAsync(IEnumerable<TKey> keys)
    {
        _selectedKeys.Clear();
        _selectedKeys.AddRange(keys);
        await SelectionChanged.InvokeAsync(_selectedKeys).ConfigureAwait(true);
    }

    private async Task UpdateSelectionAsync(MouseEventArgs args, TItem item)
    {
        // if multiple selection and check boxes then row click does not adjust selection
        if (GetItemKey is null || (SelectionMode == SelectionModes.Multiple && ShowCheckboxes))
        {
            return;
        }

        var key = GetItemKey(item);
        switch (SelectionMode)
        {
            case SelectionModes.None:
                return;

            case SelectionModes.Single:
                if (_selectedKeys.Contains(key))
                {
                    return;
                }
                _selectedKeys.Clear();
                _selectedKeys.Add(key);
                break;

            case SelectionModes.Multiple:
                if (args.CtrlKey)
                {
                    if (_selectedKeys.Contains(key))
                    {
                        _selectedKeys.Remove(key);
                    }
                    else
                    {
                        _selectedKeys.Add(key);
                    }
                }
                else
                {
                    _selectedKeys.Clear();
                    _selectedKeys.Add(key);
                }
                break;
        }
        await SelectionChanged.InvokeAsync(_selectedKeys).ConfigureAwait(true);
    }
}
