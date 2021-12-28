namespace Bluefish.Blazor.Components;

public partial class BfToggle
{
    [Parameter]
    public string CssClass { get; set; } = "btn";

    [Parameter]
    public string OffIconCssClass { get; set; } = "";

    [Parameter]
    public string OffText { get; set; } = "Off";

    [Parameter]
    public string OnIconCssClass { get; set; } = "";

    [Parameter]
    public string OnText { get; set; } = "On";

    [Parameter]
    public bool Value { get; set; }

    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    private async Task OnClickAsync(MouseEventArgs _)
    {
        Value = !Value;
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(false);
    }
}
