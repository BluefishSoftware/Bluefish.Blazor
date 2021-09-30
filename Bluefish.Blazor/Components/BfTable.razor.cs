using Bluefish.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bluefish.Blazor.Components
{
    public partial class BfTable<TItem, TKey> : IDisposable
    {
        private readonly List<BfColumn<TItem, TKey>> _columns = new();
        private readonly List<TKey> _selectedKeys = new();
        private IEnumerable<TItem> _dataItems;

        [Parameter]
        public bool AutoLoad { get; set; } = true;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; } = "table-striped";

        [Parameter]
        public Func<TItem, TKey> GetItemKey { get; set; }

        [Parameter]
        public Func<PageInfo, Task<IEnumerable<TItem>>> GetPagedDataAsync { get; set; }

        [Parameter]
        public Func<IEnumerable<TKey>, Task<IEnumerable<TItem>>> GetSelectedDataAsync { get; set; }

        [CascadingParameter]
        public BfLoadingView LoadingView { get; set; }

        [Parameter]
        public RenderFragment NoDataTemplate { get; set; }

        [Parameter]
        public PageInfo PageInfo { get; set; }

        [Parameter]
        public EventCallback<IEnumerable<TKey>> SelectionChanged { get; set; }

        [Parameter]
        public SelectionModes SelectionMode { get; set; }

        [Parameter]
        public bool ShowPager { get; set; }

        [Parameter]
        public Func<TItem, string> TrCssClass { get; set; }

        public BfColumn<TItem, TKey>[] VisibleColumns => _columns.Where(x => x.IsVisible).ToArray();

        public void AddColumn(BfColumn<TItem, TKey> column)
        {
            _columns.Add(column);
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
            if (GetItemKey != null)
            {
                var key = GetItemKey(item);
                return _selectedKeys.Contains(key) ? "selected" : string.Empty;
            }
            return string.Empty;
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

        protected override void OnInitialized()
        {
            if (PageInfo != null)
            {
                PageInfo.Changed += PageInfo_Changed;
            }
        }

        private async Task OnRowClickAsync(MouseEventArgs args, TItem item)
        {
            if (GetItemKey != null)
            {
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

        private async void PageInfo_Changed(object sender, EventArgs e)
        {
            await InvokeAsync(RefreshAsync).ConfigureAwait(true);
        }

        public async Task RefreshAsync()
        {
            LoadingView?.StartLoading();
            if (GetPagedDataAsync is null)
            {
                _dataItems = null;
            }
            else
            {
                _dataItems = await GetPagedDataAsync(PageInfo).ConfigureAwait(true);
            }
            LoadingView?.StopLoading();
            StateHasChanged();
        }

        public IEnumerable<TKey> SelectionKeys
        {
            get
            {
                return _selectedKeys;
            }
        }
    }
}
