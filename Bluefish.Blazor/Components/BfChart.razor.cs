namespace Bluefish.Blazor.Components;

public partial class BfChart : IDisposable
{
    private DotNetObjectReference<BfChart> _objRef;
    private IJSObjectReference _module;
    private IJSObjectReference _chart;
    private ElementReference _canvasElement;
    private static int _seq;
    private int _dataHashCode;
    private int _modelHashCode;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public string Height { get; set; } = "100px";

    [Parameter]
    public string Id { get; set; } = $"chart-{++_seq}";

    [Parameter]
    public string Width { get; set; } = "100px";

    [Parameter]
    public Chart Model { get; set; }

    public void Dispose()
    {
        if (_chart != null)
        {
            _chart.InvokeVoidAsync("destroy");
            //Console.WriteLine($"Chart: {Id} destroyed - dispose");
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _objRef = DotNetObjectReference.Create(this);
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfChart.razor.js").ConfigureAwait(true);
            _chart = await _module.InvokeAsync<IJSObjectReference>("initialize", _canvasElement, Model, _objRef).ConfigureAwait(true);
            _modelHashCode = Model.GetHashCode();
            _dataHashCode = Model.Data.GetHashCode();
            //Console.WriteLine($"Chart: {Id} created");
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        //Console.WriteLine($"OnParametersSetAsync: Chart: {Id} Height: {Height}, Width: {Width}, CssClass: {CssClass}, Model: {Model.GetHashCode()}");
        if (_chart != null)
        {
            if (Model.GetHashCode() != _modelHashCode)
            {
                // destroy and re-create chart
                await _chart.InvokeVoidAsync("destroy").ConfigureAwait(true);
                //Console.WriteLine($"Chart: {Id} destroyed");
                _chart = await _module.InvokeAsync<IJSObjectReference>("initialize", _canvasElement, Model, _objRef).ConfigureAwait(true);
                _modelHashCode = Model.GetHashCode();
                //Console.WriteLine($"Chart: {Id} created");
            }
            else if (Model.Data.GetHashCode() != _dataHashCode)
            {
                // update data
                await _module.InvokeVoidAsync("update", _chart, Model.Data).ConfigureAwait(true);
                _dataHashCode = Model.Data.GetHashCode();
                //Console.WriteLine($"Chart: {Id} data updated");
            }
        }
    }
}
