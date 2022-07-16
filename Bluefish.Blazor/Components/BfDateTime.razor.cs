namespace Bluefish.Blazor.Components;

public partial class BfDateTime
{
    [Parameter]
    public string CssClass { get; set; } = "form-control form-control-sm";

    [Parameter]
    public DateTime Value { get; set; }

    [Parameter]
    public EventCallback<DateTime> ValueChanged { get; set; }

    private async Task OnDateChanged(ChangeEventArgs args)
    {
        if (DateTime.TryParse(args.Value.ToString(), out var dt))
        {
            Value = new DateTime(dt.Year, dt.Month, dt.Day, Value.Hour, Value.Minute, Value.Second, DateTimeKind.Local);
            await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
        }
    }

    private async Task OnTimeChanged(ChangeEventArgs args)
    {
        if (TimeSpan.TryParse(args.Value.ToString(), out var ts))
        {
            Value = new DateTime(Value.Year, Value.Month, Value.Day, ts.Hours, ts.Minutes, ts.Seconds, DateTimeKind.Local);
            await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
        }
    }
}

