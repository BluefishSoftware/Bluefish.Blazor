using System.Globalization;

namespace Bluefish.Blazor.Components;

public partial class BfColumn<TItem, TKey>
{
    private Lazy<Func<TItem, object>> _dataMemberGetter;

    [Parameter]
    public Alignment Align { get; set; }

    [Parameter]
    public Func<TItem, bool> AllowEdit { get; set; } = (_) => true;

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public string CustomFormat { get; set; } = string.Empty;

    [Parameter]
    public Expression<Func<TItem, object>> DataMember { get; set; }

    [Parameter]
    public string DataSubMember { get; set; }

    [Parameter]
    public Type DataType { get; set; }

    [Parameter]
    public EditOptions EditOptions { get; set; } = new();

    [Parameter]
    public Func<TItem, string> GetEditValue { get; set; } = (_) => "";

    [Parameter]
    public string FilterKey { get; set; } = string.Empty;

    [Parameter]
    public string FooterCssClass { get; set; } = string.Empty;

    [Parameter]
    public string HeaderCssClass { get; set; }

    [Parameter]
    public string HeaderText { get; set; }

    [Parameter]
    public string HeaderToolTip { get; set; }

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public bool IsSortable { get; set; } = true;

    [Parameter]
    public bool IsVisible { get; set; } = true;

    [Parameter]
    public Func<string, object> EditConversion { get; set; } = (v) => v;

    [Parameter]
    public Action<TItem, object> ApplyEdit { get; set; }

    [Parameter]
    public string Width { get; set; } = string.Empty;

    [CascadingParameter(Name = "Table")]
    public BfTable<TItem, TKey> Table { get; set; }

    [Parameter]
    public RenderFragment<TItem> Template { get; set; }

    [Parameter]
    public Func<TItem, string> ToolTip { get; set; }

    public Func<TItem, object> DataMemberFunc => _dataMemberGetter.Value;

    public string FormatValue(TItem item)
    {
        var value = GetValue(item);
        if (value is null)
        {
            return string.Empty;
        }
        return (!string.IsNullOrWhiteSpace(CustomFormat) && value is IFormattable formattable)
            ? formattable.ToString(CustomFormat, CultureInfo.CurrentCulture)
            : value.ToString();
    }

    public string GetBodyCssClass()
    {
        return $"{Id} {CssClass} {GetCssClass(Align)}";
    }

    public string GetBodyStyle()
    {
        return string.IsNullOrWhiteSpace(Width) ? string.Empty : $"width: {Width}";
    }


    public string GetCssClass(Alignment align) => align switch
    {
        Alignment.Start => "text-start",
        Alignment.Center => "text-center",
        Alignment.End => "text-end",
        _ => string.Empty
    };

    public string GetFooterCssClass()
    {
        return $"{Id} {FooterCssClass} {GetCssClass(Align)}";
    }

    public string GetFooterStyle()
    {
        return string.IsNullOrWhiteSpace(Width) ? string.Empty : $"width: {Width}";
    }

    public string GetHeaderCssClass(BfTable<TItem, TKey> table)
    {
        return $"{Id} {HeaderCssClass} {(table.AllowSort && IsSortable ? "cursor-pointer" : "")} {GetCssClass(Align)}";
    }

    public string GetHeaderStyle()
    {
        return string.IsNullOrWhiteSpace(Width) ? string.Empty : $"width: {Width}";
    }

    public object GetValue(TItem item)
    {
        if (_dataMemberGetter is null)
        {
            return null;
        }
        else
        {
            try
            {
                return DataMemberFunc(item);
            }
            catch
            {
                return null;
            }
        }
    }

    public void SetValue(TItem item, string value)
    {
        if (EditOptions.IsEditable)
        {
            var typedValue = EditConversion?.Invoke(value);
            DataMember?.GetPropertyInfo().SetValue(item, typedValue);
            ApplyEdit?.Invoke(item, typedValue);
        }
    }

    public void SetIsVisible(bool visible)
    {
        IsVisible = visible;
    }

    protected override void OnInitialized()
    {
        Table?.AddColumn(this);
        if (DataMember != null)
        {
            _dataMemberGetter = new Lazy<Func<TItem, object>>(() => DataMember.Compile());
            GetEditValue = (item) => GetValue(item)?.ToString() ?? string.Empty;
        }
    }
}
