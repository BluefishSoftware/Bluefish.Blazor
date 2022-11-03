using System.Threading;

namespace Bluefish.Blazor.Components;

public partial class BfCopyToClipboardButton : IAsyncDisposable
{
    private Timer _timer;
    private IJSObjectReference _module;
    private bool _provideFeedback;
    private const int FEEDBACK_PERIOD = 250;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public Dictionary<string, object> Attributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> Click { get; set; }

    [Parameter]
    public Func<string> FetchData { get; set; } = () => string.Empty;

    [Parameter]
    public string IconCssClass { get; set; } = "fas fa-copy";

    [Parameter]
    public bool IsPrimary { get; set; }

    [Parameter]
    public bool IsSubmit { get; set; }

    [Parameter]
    public bool PreventDefault { get; set; }

    [Parameter]
    public Sizes Size { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; } = "d-none d-sm-inline-block";

    [Parameter]
    public string Title { get; set; } = "Copy to clipboard";

    private Dictionary<string, object> ActualAttributes
    {
        get
        {
            var attr = new Dictionary<string, object>(Attributes ?? new())
                {
                    { "disabled", Enabled || string.IsNullOrWhiteSpace(FetchData?.Invoke()) ? null : true },
                    { "class", $"bf-button btn {Size.CssClass("btn-sm", "", "btn-lg")} {(IsPrimary ? "btn-primary" : "")} {CssClass}" }
                };
            if (!Visible)
            {
                attr.Add("style", "display: none;");
            }
            return attr;
        }
    }

    public string ActualIconCssClass => _provideFeedback ? $"{IconCssClass} fade" : IconCssClass;

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_module != null)
            {
                await _module.DisposeAsync().ConfigureAwait(false);
            }
            if (_timer != null)
            {
                await _timer.DisposeAsync().ConfigureAwait(false); ;
            }
        }
        catch
        {
        }
    }

    private async Task OnClick()
    {
        // fetch data to copy to clipboard
        var data = FetchData?.Invoke();
        if (!string.IsNullOrWhiteSpace(data))
        {
            // copy to clipboard
            await _module.InvokeVoidAsync("copy", data);

            // provide feedback
            _provideFeedback = true;
            _timer.Change(FEEDBACK_PERIOD, Timeout.Infinite);
        }

        // notify
        await Click.InvokeAsync().ConfigureAwait(true);
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _timer = new Timer(TimerCallback);
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfCopyToClipboardButton.razor.js").ConfigureAwait(true);
        }
    }

    private async void TimerCallback(object state)
    {
        _provideFeedback = false;
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        await InvokeAsync(() => StateHasChanged()).ConfigureAwait(true);
    }
}
