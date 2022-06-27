namespace Bluefish.Blazor.Components;

public partial class BfTimeSpanEditor
{
    private int _days;
    private int _hours;
    private int _minutes;
    private int _seconds;

    [Parameter]
    public TimeSpan Value { get; set; }

    [Parameter]
    public EventCallback<TimeSpan> ValueChanged { get; set; }

    private Task OnDaysChanged(ChangeEventArgs args)
    {
        if (int.TryParse(args.Value?.ToString() ?? "0", out int days))
        {
            _days = days;
            return OnValueChangedAsync();
        }
        return Task.CompletedTask;
    }

    private Task OnHoursChanged(ChangeEventArgs args)
    {
        if (int.TryParse(args.Value?.ToString() ?? "0", out int hours))
        {
            if (hours >= 0 && hours <= 23)
            {
                _hours = hours;
                return OnValueChangedAsync();
            }
        }
        return Task.CompletedTask;
    }

    private Task OnMinutesChanged(ChangeEventArgs args)
    {
        if (int.TryParse(args.Value?.ToString() ?? "0", out int mins))
        {
            if (mins >= 0 && mins <= 59)
            {
                _minutes = mins;
                return OnValueChangedAsync();
            }
        }
        return Task.CompletedTask;
    }


    private Task OnSecondsChanged(ChangeEventArgs args)
    {
        if (int.TryParse(args.Value?.ToString() ?? "0", out int secs))
        {
            if (secs >= 0 && secs <= 59)
            {
                _seconds = secs;
                return OnValueChangedAsync();
            }
        }
        return Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        _days = Value.Days;
        _hours = Value.Hours;
        _minutes = Value.Minutes;
        _seconds = Value.Seconds;
    }

    private async Task OnValueChangedAsync()
    {
        Value = new TimeSpan(_days, _hours, _minutes, _seconds);
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }

}
