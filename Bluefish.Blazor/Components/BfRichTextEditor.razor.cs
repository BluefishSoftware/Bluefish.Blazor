namespace Bluefish.Blazor.Components;

public partial class BfRichTextEditor : IAsyncDisposable
{
    private static int _sequence;
    private DotNetObjectReference<BfRichTextEditor> _objRef = null!;
    private IJSObjectReference _module;
    private bool _initialized;

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; }

    [Parameter] public bool Enabled { get; set; } = true;

    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    [Parameter] public string Value { get; set; } = string.Empty;

    [Parameter] public string CssClass { get; set; } = string.Empty;

    [Parameter] public int? HeightPixels { get; set; } = null;

    [Parameter] public int? WidthPixels { get; set; } = null;

    [Parameter] public bool Resize { get; set; }

    public string Id { get; } = $"text-editor-{++_sequence}";

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_module != null)
            {
                await _module.InvokeVoidAsync("dispose", Id).ConfigureAwait(true);
                await _module.DisposeAsync().ConfigureAwait(true);
            }
            _objRef?.Dispose();
        }
        catch
        {
        }
    }

    protected override async Task OnInitializedAsync()
    {
        _objRef = DotNetObjectReference.Create<BfRichTextEditor>(this);
        _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfRichTextEditor.razor.js").ConfigureAwait(true);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_initialized && JSRuntime != null && _module != null)
        {
            _initialized = true;
            await _module.InvokeVoidAsync("initialize",
                Id,
                Value,
                _objRef,
                HeightPixels,
                WidthPixels,
                Resize).ConfigureAwait(true);
        }
    }

    [JSInvokable]
    public async Task OnChange(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }

    //public async Task SetValueAsync(string value)
    //{
    //    Value = value;
    //    await JSRuntime.InvokeVoidAsync("SetTextEditorValue", Value).ConfigureAwait(true);
    //    await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    //}
}
