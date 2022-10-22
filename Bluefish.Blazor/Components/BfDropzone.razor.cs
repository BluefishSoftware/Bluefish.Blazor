namespace Bluefish.Blazor.Components;

public partial class BfDropzone : IAsyncDisposable
{
    private DotNetObjectReference<BfDropzone> _objRef;
    private IJSObjectReference _module;
    private bool _initialized;

    [Parameter]
    public string AcceptedFiles { get; set; }

    [Parameter]
    public EventCallback AllUploadsComplete { get; set; }

    [Parameter]
    public string CssClass { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    [Parameter]
    public int MaxFilesize { get; set; } = 50;

    [Parameter]
    public int Timeout { get; set; } = 30000;

    [Parameter]
    public string Url { get; set; }

    protected async override Task OnInitializedAsync()
    {
        _objRef = DotNetObjectReference.Create<BfDropzone>(this);
        _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfDropzone.razor.js").ConfigureAwait(true);
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_initialized && JSRuntime != null && _module != null)
        {
            _initialized = true;
            var options = new
            {
                Url,
                Timeout,
                MaxFilesize,
                AcceptedFiles,
                PreviewItemTemplate = "#my-dropzone-template",
                Headers = Headers
            };
            await _module.InvokeVoidAsync("initialize", "#my-dropzone", options, _objRef).ConfigureAwait(true);
        }
    }

    private async void OnClear()
    {
        await _module.InvokeVoidAsync("clear", "#my-dropzone").ConfigureAwait(true);
    }

    [JSInvokable("Bluefish.Blazor.BfDropzone.OnAllUploadsComplete")]
    public async Task OnAllUploadsComplete()
    {
        await AllUploadsComplete.InvokeAsync(null).ConfigureAwait(true);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_module != null)
        {
            await _module.InvokeVoidAsync("setUrl", Url).ConfigureAwait(true);
        }
    }

    //private async Task OnClick()
    //{
    //    await _module.InvokeVoidAsync("click", "#my-dropzone").ConfigureAwait(true);
    //}

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_module != null)
            {
                await _module.DisposeAsync().ConfigureAwait(false);
            }
            _objRef?.Dispose();
        }
        catch
        {
        }
    }
}
