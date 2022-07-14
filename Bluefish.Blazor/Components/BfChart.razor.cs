namespace Bluefish.Blazor.Components;

public partial class BfChart : IDisposable
{
    private DotNetObjectReference<BfChart> _objRef;
    private IJSObjectReference _module;
    private IJSObjectReference _chart;
    private ElementReference _canvasElement;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public string Height { get; set; } = "100px";

    [Parameter]
    public string Width { get; set; } = "100px";

    [Parameter]
    public Chart Model { get; set; }

    public void Dispose()
    {
        if (_chart != null)
        {
            _chart.InvokeVoidAsync("destroy");
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _objRef = DotNetObjectReference.Create(this);
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfChart.razor.js").ConfigureAwait(true);
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Model != null && _module != null)
        {
            if (_chart != null)
            {
                await _chart.InvokeVoidAsync("destroy").ConfigureAwait(true);
                _chart = null;
            }
            _chart = await _module.InvokeAsync<IJSObjectReference>("initialize", _canvasElement, Model, _objRef).ConfigureAwait(true);
        }
    }
}
