using Bluefish.Blazor.Utility;
using Microsoft.AspNetCore.WebUtilities;

namespace Bluefish.Blazor.Components;

public partial class BfTable<TItem, TKey> : IAsyncDisposable
{
    private bool _isLoading = true;
    private IEnumerable<TItem> _dataItems;
    private IJSObjectReference _commonModule;
    private readonly List<TKey> _selectedKeys = new();
    private readonly List<BfColumn<TItem, TKey>> _columns = new();
    private FilterInfo _previousFilterInfo = null;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    #region parameters

    [Parameter]
    public bool AllowSort { get; set; } = false;

    [Parameter]
    public bool AutoLoad { get; set; } = true;

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public object DataSource { get; set; }

    [Parameter]
    public TableEditModes EditMode { get; set; }

    [Parameter]
    public EventCallback<TableEditInfo<TKey>> CellEdited { get; set; }

    [Parameter]
    public FilterInfo FilterInfo { get; set; } = new();

    [Parameter]
    public Func<TItem, TKey> GetItemKey { get; set; } = (_) => default;

    [Parameter]
    public Func<BfColumn<TItem, TKey>, IEnumerable<TItem>, string> GetFooterTextAsync { get; set; }

    [Parameter]
    public Func<PageInfo, SortInfo, FilterInfo, Task<IEnumerable<TItem>>> GetPagedDataAsync { get; set; }

    [Parameter]
    public Func<IEnumerable<TKey>, Task<IEnumerable<TItem>>> GetSelectedDataAsync { get; set; }

    [Parameter]
    public bool IgnoreRowOnClickWhenCheckboxes { get; set; }

    [Parameter]
    public RenderFragment NoDataTemplate { get; set; }

    [Parameter]
    public PageInfo PageInfo { get; set; }

    [Parameter]
    public string QueryStringPrefix { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<TItem> RowClick { get; set; }

    [Parameter]
    public EventCallback<TItem> RowDoubleClick { get; set; }

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
    public bool ShowSummaryRow { get; set; }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public SortInfo SortInfo { get; set; }

    [Parameter]
    public Func<TItem, string> TrCssClass { get; set; } = (_) => string.Empty;

    #endregion

    public BfColumn<TItem, TKey>[] AllColumns => _columns.ToArray();

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

    private EditOptions GetEditOptions(TItem item, BfColumn<TItem, TKey> column)
    {
        return new EditOptions
        {
            IsEditable = column.EditOptions.IsEditable && column.AllowEdit(item),
            DecimalPlaces = column.EditOptions.DecimalPlaces,
            IsNumber = column.EditOptions.IsNumber,
            Format = column.EditOptions.Format,
            Required = column.EditOptions.Required
        };
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

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (FilterInfo != null)
            {
                FilterInfo.FilterChanged -= FilterInfo_FilterChanged;
            }
            if (PageInfo != null)
            {
                PageInfo.Changed -= PageInfo_Changed;
            }
            if (_commonModule != null)
            {
                await _commonModule.DisposeAsync().ConfigureAwait(true);
            }
        }
        catch
        {
        }
    }

    private async void FilterInfo_FilterChanged(object sender, EventArgs e)
    {
        await UpdateUriAsync().ConfigureAwait(true);
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
        if (!(IgnoreRowOnClickWhenCheckboxes && SelectionMode == SelectionModes.Multiple && ShowCheckboxes))
        {
            var key = GetItemKey(item);
            sb.Append(_selectedKeys.Contains(key) ? "selected " : string.Empty);
        }

        return sb.ToString()?.Trim();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _commonModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/js/common.js").ConfigureAwait(true);

            // Get the requested table parameters from the QueryString
            var uri = new Uri(NavigationManager.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);

            // get initial page info
            if (PageInfo != null)
            {
                var pageSize = PageInfo.PageSize;
                var pageNumber = PageInfo.Page;
                if (query.TryGetValue($"{QueryStringPrefix}ps", out var val1))
                {
                    pageSize = Convert.ToInt32(val1);
                }
                if (query.TryGetValue($"{QueryStringPrefix}p", out var val2))
                {
                    pageNumber = Convert.ToInt32(val2);
                }
                PageInfo.Init(pageSize, pageNumber);
                PageInfo.Changed += PageInfo_Changed;
            }

            // get initial sort info
            if (SortInfo != null)
            {
                var columnId = SortInfo.ColumnId;
                var direction = SortInfo.Direction;
                if (query.TryGetValue($"{QueryStringPrefix}s", out var val1))
                {
                    columnId = val1.ToString();
                }
                if (query.TryGetValue($"{QueryStringPrefix}sd", out var val2))
                {
                    direction = Enum.Parse<SortDirections>(val2.ToString());
                }
                SortInfo.ColumnId = columnId;
                SortInfo.Direction = direction;
            }

            // get initial filter info
            if (FilterInfo != null)
            {
                if (query.TryGetValue($"{QueryStringPrefix}q", out var val1))
                {
                    FilterInfo.SearchText = val1;
                }
            }

            if (AutoLoad)
            {
                await RefreshAsync().ConfigureAwait(true);
            }
        }
    }

    private async Task OnAllCheckboxInput(bool isChecked)
    {
        if (GetItemKey != null)
        {
            var changes = false;
            foreach (var item in _dataItems)
            {
                var key = GetItemKey(item);
                if (isChecked)
                {
                    if (!_selectedKeys.Contains(key))
                    {
                        _selectedKeys.Add(key);
                        changes = true;
                    }
                }
                else if (_selectedKeys.Contains(key))
                {
                    _selectedKeys.Remove(key);
                    changes = true;
                }
            }
            if (changes)
            {
                await SelectionChanged.InvokeAsync(_selectedKeys).ConfigureAwait(true);
            }
        }
    }

    private async Task OnApplyEditAsync(TItem item, BfColumn<TItem, TKey> col, string value)
    {
        // attempt to apply to item
        if (col.EditOptions.IsEditable)
        {
            // get current / old value
            var oldValue = col.GetValue(item);

            // get typed value
            var typedValue = col.EditConversion?.Invoke(value);

            // apply to item
            col.DataMember?.GetPropertyInfo().SetValue(item, typedValue);

            // apply to backend?
            if (DataSource is IDataUpdater<TItem> dp)
            {
                var members = PropertyPath<TItem>.Get(col.DataMember);
                if (members.Count > 2)
                {
                    throw new Exception("Unable to update nested properties");
                }
                var property = string.Join(".", members.Select(p => p.Name));
                await dp.UpdateAsync(item, new Dictionary<string, object> { { property, typedValue } }).ConfigureAwait(true);
            }

            // notify cell has been edited
            if (GetItemKey != null)
            {
                await CellEdited.InvokeAsync(new TableEditInfo<TKey>
                {
                    ItemId = GetItemKey(item),
                    ColumnId = col.Id,
                    NewValue = typedValue,
                    OldValue = oldValue
                });
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

    protected async Task OnColumnHeaderClick(BfColumn<TItem, TKey> column)
    {
        if (AllowSort && column.IsSortable && SortInfo != null)
        {
            // toggle sort
            SortInfo.Direction = SortInfo.ColumnId == column.Id
                ? (SortInfo.Direction == SortDirections.Descending ? SortDirections.Ascending : SortDirections.Descending)
                : SortDirections.Ascending;
            SortInfo.ColumnId = column.Id;

            // update current uri with sorting info
            await UpdateUriAsync().ConfigureAwait(true);

            // refresh table
            await RefreshAsync().ConfigureAwait(true);
        }
    }

    protected override void OnInitialized()
    {
        FilterInfo.FilterChanged += FilterInfo_FilterChanged;
        _previousFilterInfo = FilterInfo;
    }

    protected override void OnParametersSet()
    {
        if (FilterInfo != _previousFilterInfo)
        {
            if (_previousFilterInfo != null)
            {
                _previousFilterInfo.FilterChanged += FilterInfo_FilterChanged;
            }
            if (FilterInfo != null)
            {
                FilterInfo.FilterChanged += FilterInfo_FilterChanged;
                _previousFilterInfo = FilterInfo;
            }
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
        // update current uri with paging info
        await UpdateUriAsync().ConfigureAwait(true);

        // refresh table
        await InvokeAsync(RefreshAsync).ConfigureAwait(true);
    }

    public async Task RefreshAsync()
    {
        _isLoading = true;
        if (GetPagedDataAsync is null)
        {
            if (DataSource is IDataSource<TItem> ds)
            {
                _dataItems = await ds.FetchAsync(PageInfo, SortInfo, FilterInfo).ConfigureAwait(true);
            }
            else if (DataSource is IEnumerable<TItem> list)
            {
                if (PageInfo != null)
                {
                    PageInfo.TotalCount = list.Count();
                    _dataItems = list.Skip(PageInfo.PageRangeStart - 1).Take(PageInfo.PageSize);
                }
                else
                {
                    _dataItems = list;
                }
            }
            else
            {
                _dataItems = Array.Empty<TItem>();
            }
        }
        else
        {
            _dataItems = await GetPagedDataAsync(PageInfo, SortInfo, FilterInfo).ConfigureAwait(true);
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
        if (GetItemKey is null || (IgnoreRowOnClickWhenCheckboxes && SelectionMode == SelectionModes.Multiple && ShowCheckboxes))
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

    private async Task UpdateUriAsync()
    {
        // update current uri with paging and sorting info
        var dict = new Dictionary<string, object>();

        if (PageInfo != null)
        {
            if (PageInfo.Page != 1)
            {
                dict.Add($"{QueryStringPrefix}p", PageInfo.Page);
            }
            if (PageInfo.PageSize != PageInfo.DEFAULT_PAGE_SIZE)
            {
                dict.Add($"{QueryStringPrefix}ps", PageInfo.PageSize);
            }
        };

        if (SortInfo != null)
        {
            if (!string.IsNullOrWhiteSpace(SortInfo.ColumnId))
            {
                dict.Add($"{QueryStringPrefix}s", SortInfo.ColumnId);
            }
            if (SortInfo.Direction != SortDirections.Ascending)
            {
                dict.Add($"{QueryStringPrefix}sd", SortInfo.Direction.ToString());
            }
        }

        if (FilterInfo != null)
        {
            if (string.IsNullOrWhiteSpace(FilterInfo.SearchText))
            {
                dict.Remove($"{QueryStringPrefix}q");
            }
            else
            {
                dict.Add($"{QueryStringPrefix}q", FilterInfo.SearchText);
            }
        }

        var newUri = NavigationManager.GetUriWithQueryParameters(dict);
        await _commonModule.InvokeVoidAsync("replaceUrl", newUri).ConfigureAwait(true);
    }
}
