namespace Bluefish.Blazor.Components;

public partial class BfCheckBox
{
    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public bool PreventDefault { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; }

    [Parameter]
    public bool Value { get; set; }

    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    private async Task OnChange(ChangeEventArgs args)
    {
        Value = System.Convert.ToBoolean(args.Value);
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }
}
