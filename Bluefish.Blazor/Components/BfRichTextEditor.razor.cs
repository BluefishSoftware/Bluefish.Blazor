namespace Bluefish.Blazor.Components;

public partial class BfRichTextEditor : IDisposable
{
    private static int _sequence;
    private DotNetObjectReference<BfRichTextEditor> _jsThisRef = null!;
    private IJSObjectReference _jsModule;
    private bool _initialized;

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter] public bool Enabled { get; set; } = true;

    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    [Parameter] public string Value { get; set; } = string.Empty;

    [Parameter] public string CssClass { get; set; } = string.Empty;

    [Parameter] public int? HeightPixels { get; set; } = null;

    [Parameter] public int? WidthPixels { get; set; } = null;

    [Parameter] public bool Resize { get; set; }

    public string Id { get; } = $"TextEditor_{++_sequence}";

    protected override async Task OnInitializedAsync()
    {
        _jsThisRef = DotNetObjectReference.Create<BfRichTextEditor>(this);
        _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/js/rich-text-editor.js").ConfigureAwait(true);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_initialized && JSRuntime != null && _jsModule != null)
        {
            _initialized = true;
            await _jsModule.InvokeVoidAsync("InitialiseTextEditor",
                Id,
                Value,
                _jsThisRef,
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

    public void Dispose()
    {
        _jsModule.InvokeVoidAsync("RemoveTextEditor", Id);
        _jsThisRef?.Dispose();
    }
}
