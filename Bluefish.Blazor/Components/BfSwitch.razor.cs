namespace Bluefish.Blazor.Components;

public partial class BfSwitch
{
    private static int _seq;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-toggle-{++_seq}";

    [Parameter]
    public bool Enabled { get; set; } = true;

    [Parameter]
    public string OffLabel { get; set; } = "Off";

    [Parameter]
    public string OffStyle { get; set; } = string.Empty;

    [Parameter]
    public string OnLabel { get; set; } = "On";

    [Parameter]
    public string OnStyle { get; set; } = string.Empty;

    [Parameter]
    public string Size { get; set; } = "normal";

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public bool TextBeforeSwitch { get; set; }

    [Parameter]
    public string TextCssClass { get; set; }

    [Parameter] public bool Value { get; set; }

    [Parameter] public EventCallback<bool> ValueChanged { get; set; }

    private async Task OnChangeAsync(ChangeEventArgs args)
    {
        Value = args.Value?.ToString() == "True";
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }
}
