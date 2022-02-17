namespace Bluefish.Blazor.Components;

public partial class BfRadioButtons
{
    private static int _seq = 0;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-button-group-{++_seq}";

    [Parameter]
    public string OptionCssClass { get; set; } = "btn btn-outline-secondary";

    [Parameter]
    public IEnumerable<Option> Options { get; set; } = Array.Empty<Option>();

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    protected Dictionary<string, object> ItemAttributes(Option option)
    {
        return new Dictionary<string, object>
        {
            ["checked"] = Value == option.Value,
            ["disabled"] = !option.Enabled,
            ["name"] = Id
        };
    }

    protected async Task OnItemClick(Option option)
    {
        Value = option.Value;
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }

    protected string ButtonSizeCssClass
    {
        get
        {
            return Size switch
            {
                Sizes.Small => "btn-sm",
                Sizes.Large => "btn-lg",
                _ => string.Empty,
            };
        }
    }
}
