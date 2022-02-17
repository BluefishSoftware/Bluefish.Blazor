namespace Bluefish.Blazor.Components;

public partial class BfDateRange
{
    private static int _sequence;
    private BfDropDown _dropDown;

    [Parameter]
    public Dictionary<string, object> Attributes { get; set; }

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public Directions DropdownDirection { get; set; } = Directions.Down;

    [Parameter]
    public bool Enabled { get; set; } = true;

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-daterange-{++_sequence}";

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public string TextCssClass { get; set; }

    [Parameter]
    public DateRange Value { get; set; } = new DateRange();

    [Parameter]
    public EventCallback<DateRange> ValueChanged { get; set; }

    [Parameter]
    public bool Visible { get; set; } = true;

    private Dictionary<string, object> ActualAttributes
    {
        get
        {
            var attr = new Dictionary<string, object>(Attributes ?? new())
            {
                //{ "class", $"bf-button btn {FormControlSizeCssClass} {CssClass}" }
            };
            return attr;
        }
    }

    private string FormControlSizeCssClass
    {
        get
        {
            return Size == Sizes.Small ? "form-control-sm" : Size == Sizes.Large ? "form-control-lg" : "";
        }
    }

    private async Task OnDateFromChangeAsync(ChangeEventArgs args)
    {
        Value.DateFrom = Convert.ToDateTime(args.Value);
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }

    private async Task OnDateToChangeAsync(ChangeEventArgs args)
    {
        Value.DateTo = Convert.ToDateTime(args.Value);
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }

    private async Task SetValueAsync(DateRange value)
    {
        Value = value;
        var tasks = new List<Task>
            {
                ValueChanged.InvokeAsync(Value),
                _dropDown.HideAsync()
            };
        await Task.WhenAll(tasks).ConfigureAwait(true);
    }
}
