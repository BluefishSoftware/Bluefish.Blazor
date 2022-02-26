namespace Bluefish.Blazor.Components;

public partial class BfDropDown : IAsyncDisposable
{
    private static int _sequence;
    private bool _shown;
    private IJSObjectReference _module;
    private IJSObjectReference _dropdown;
    private DotNetObjectReference<BfDropDown> _objRef;

    public enum CloseOptions
    {
        Inside,
        Outside,
        InsideOrOutside,
        Manual
    }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public RenderFragment ButtonContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> Click { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public CloseOptions CloseOption { get; set; } = CloseOptions.Outside;

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public Directions DropdownDirection { get; set; } = Directions.Down;

    [Parameter]
    public bool Enabled { get; set; } = true;

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-dropdown-{++_sequence}";

    [Parameter]
    public bool PreventDefault { get; set; }

    [Parameter]
    public bool ShowCaret { get; set; } = true;

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public bool Visible { get; set; } = true;

    private Dictionary<string, object> Attributes
    {
        get
        {
            return new Dictionary<string, object>
                {
                    { "data-bs-toggle", "dropdown" },
                    { "data-bs-auto-close", CloseOption switch
                    {
                        CloseOptions.Inside  => "inside",
                        CloseOptions.Outside  => "outside",
                        CloseOptions.InsideOrOutside  => "true",
                        _ => "false"
                    } }
                };
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_dropdown != null)
            {
                await _dropdown.DisposeAsync().ConfigureAwait(false);
            }
            if (_module != null)
            {
                await _module.DisposeAsync().ConfigureAwait(false);
            }
        }
        catch
        {
        }
    }

    public async Task HideAsync()
    {
        await _dropdown.InvokeVoidAsync("hide").ConfigureAwait(true);
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _objRef = DotNetObjectReference.Create(this);
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", NavigationManager.ToAbsoluteUri("_content/Bluefish.Blazor/js/interop.js").AbsolutePath).ConfigureAwait(true);
            _dropdown = await _module.InvokeAsync<IJSObjectReference>("initDropDown", Id, _objRef).ConfigureAwait(true);
        }
    }

    [JSInvokable]
    public void OnDropDownShown()
    {
        _shown = true;
        StateHasChanged();
    }

    [JSInvokable]
    public void OnDropDownHidden()
    {
        _shown = false;
        StateHasChanged();
    }

    private string ParentCssClasses
    {
        get
        {
            var dropdownPosition = DropdownDirection switch
            {

                Directions.Up => "dropup",
                Directions.Left => "dropstart",
                Directions.Right => "dropend",
                _ => ""
            };
            return $"{dropdownPosition}";
        }
    }

    public async Task ShowAsync()
    {
        await _dropdown.InvokeVoidAsync("show").ConfigureAwait(true);
    }
}