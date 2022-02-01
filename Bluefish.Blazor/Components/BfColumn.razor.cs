namespace Bluefish.Blazor.Components;

public partial class BfColumn<TItem, TKey>
{
    private Lazy<Func<TItem, object>> _dataMemberGetter;

    [Parameter]
    public Alignment Align { get; set; }

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public Expression<Func<TItem, object>> DataMember { get; set; }

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

    [CascadingParameter(Name = "Table")]
    public BfTable<TItem, TKey> Table { get; set; }

    [Parameter]
    public RenderFragment<TItem> Template { get; set; }

    [Parameter]
    public Func<TItem, string> ToolTip { get; set; }

    public Func<TItem, object> DataMemberFunc => _dataMemberGetter.Value;

    private static string GetCssClass(Alignment align) => align switch
    {
        Alignment.Start => "text-start",
        Alignment.Center => "text-center",
        Alignment.End => "text-end",
        _ => string.Empty
    };
    public string GetHeaderCssClass(BfTable<TItem, TKey> table)
    {
        return $"{HeaderCssClass} {(table.AllowSort && IsSortable ? "cursor-pointer" : "")} {GetCssClass(Align)}";
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

    protected override void OnInitialized()
    {
        Table?.AddColumn(this);

        if (DataMember != null)
        {
            _dataMemberGetter = new Lazy<Func<TItem, object>>(() => DataMember.Compile());
        }
    }
}
