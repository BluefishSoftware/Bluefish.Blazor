namespace Bluefish.Blazor.Components;

public partial class BfFilterPanel
{
    private NewFilterModel _newFilter;

    [Parameter]
    public EventCallback Apply { get; set; }

    [Parameter]
    public EventCallback Close { get; set; }

    [Parameter]
    [EditorRequired]
    public FilterInfo FilterInfo { get; set; } = new();

    [Parameter]
    public EventCallback<string> FilterChanged { get; set; }

    [Parameter]
    public string[] FilterKeys { get; set; } = Array.Empty<string>();

    [Parameter]
    public Func<Filter, string> GetFieldName { get; set; } = (f) => f.Key;

    [Parameter]
    public Func<Filter, Type> GetFieldType { get; set; } = (f) => typeof(String);

    [Parameter]
    public Sizes Size { get; set; }

    private string GetDateTimeString(DateTime value)
    {
        if (value.Hour == 0 && value.Minute == 0 && value.Second == 0)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-dd");
        }
        return value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
    }

    private void OnAdd()
    {
        _newFilter = new NewFilterModel
        {
            Key = FilterKeys.FirstOrDefault() ?? String.Empty
        };
        _newFilter.DataType = GetFieldType(new Filter(_newFilter.Key));
        if (_newFilter.DataType.IsBool())
        {
            _newFilter.Values = "true";
        }
    }

    private async Task OnApply()
    {
        _newFilter = null;
        await Apply.InvokeAsync().ConfigureAwait(true);
    }

    private void OnCancelAdd()
    {
        _newFilter = null;
    }

    private async Task OnClear()
    {
        FilterInfo.Clear();
        await FilterChanged.InvokeAsync(FilterInfo.SearchText).ConfigureAwait(true);
    }

    private async Task OnClose()
    {
        _newFilter = null;
        await Close.InvokeAsync().ConfigureAwait(true);
    }

    private void OnNewFilterKeyChanged(string key)
    {
        _newFilter.Key = key;
        _newFilter.Type = FilterTypes.Equals;
        _newFilter.DataType = GetFieldType(new Filter(key));
        _newFilter.Values = _newFilter.DataType.IsBool() ? "true" : String.Empty;
    }

    private async Task OnSaveAdd()
    {
        if (_newFilter != null)
        {
            IEnumerable<string> values = Array.Empty<string>();
            if (_newFilter.DataType.IsText())
            {
                values = _newFilter.Values.ParseAsCsv().Select(x => x.AddQuotes());
                if (_newFilter.Type == FilterTypes.Range && values.Count() < 2)
                {
                    values = new string[] { string.Empty, string.Empty };
                }
                else if (values.Count() < 1)
                {
                    values = new string[] { string.Empty };
                }
            }
            else if (_newFilter.DataType.IsDate())
            {
                if (_newFilter.Type == FilterTypes.Range)
                {
                    values = new string[] { GetDateTimeString(_newFilter.Date1), GetDateTimeString(_newFilter.Date2) };
                }
                else
                {
                    values = new string[] { GetDateTimeString(_newFilter.Date1) };
                }
            }
            else if (_newFilter.DataType.IsEnum)
            {
                values = _newFilter.Values.ParseAsCsv();
            }
            else if (_newFilter.DataType.IsBool())
            {
                values = new string[] { _newFilter.Values };
            }

            // add new filter
            var filter = new Filter(_newFilter.Key, _newFilter.Type, values);
            FilterInfo.AddFilter(filter);

            // update
            await FilterChanged.InvokeAsync(FilterInfo.SearchText).ConfigureAwait(true);
            _newFilter = null;
        }
    }

    private async Task OnRemove(Filter filter)
    {
        FilterInfo.RemoveFilter(filter);
        await FilterChanged.InvokeAsync(FilterInfo.SearchText).ConfigureAwait(true);
    }

    private class NewFilterModel
    {
        public Type DataType { get; set; } = typeof(String);
        public string Key { get; set; }
        public FilterTypes Type { get; set; } = FilterTypes.Equals;
        public string Values { get; set; } = String.Empty;
        public DateTime Date1 { get; set; } = DateTime.Today;
        public DateTime Date2 { get; set; } = DateTime.Today.AddDays(1);
    }
}
