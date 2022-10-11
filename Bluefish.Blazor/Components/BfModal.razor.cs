namespace Bluefish.Blazor.Components;

public partial class BfModal : IAsyncDisposable
{
    private static int _seq;
    private DotNetObjectReference<BfModal> _objRef;
    private IJSObjectReference _commonModule;
    private IJSObjectReference _module;
    private IJSObjectReference _modal;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public EventCallback Cancel { get; set; }

    [Parameter]
    public string CancelText { get; set; } = "Cancel";

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string FocusElementId { get; set; } = String.Empty;

    [Parameter]
    public RenderFragment Footer { get; set; }

    [Parameter]
    public RenderFragment Header { get; set; }

    [Parameter]
    public EventCallback Hidden { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-modal-{++_seq}";

    [Parameter]
    public EventCallback Save { get; set; }

    [Parameter]
    public bool SaveOnEnter { get; set; } = true;

    [Parameter]
    public string SaveButtonId { get; set; } = "modal-save-button";


    [Parameter]
    public bool SaveEnabled { get; set; } = true;

    [Parameter]
    public string SaveText { get; set; } = "Save";

    [Parameter]
    public bool ShowCloseButton { get; set; }

    [Parameter]
    public bool ShowFooter { get; set; } = true;

    [Parameter]
    public bool ShowHeader { get; set; } = true;

    [Parameter]
    public EventCallback Shown { get; set; }

    [Parameter]
    public bool ShowSave { get; set; } = true;

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public string Title { get; set; }

    public Task Initialization { get; private set; }

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_modal != null)
            {
                await _modal.DisposeAsync().ConfigureAwait(false);
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
        await _modal.InvokeVoidAsync("hide").ConfigureAwait(true);
    }

    private string ModalSizeCssClass => Size switch
    {
        Sizes.Small => "modal-sm",
        Sizes.Large => "modal-lg",
        Sizes.ExtraLarge => "modal-xl",
        _ => ""
    };

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _objRef = DotNetObjectReference.Create(this);
            Initialization = Task.Run(async () =>
            {
                _commonModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/js/common.js").ConfigureAwait(true);
                _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfModal.razor.js").ConfigureAwait(true);
                _modal = await _module.InvokeAsync<IJSObjectReference>("initialize", Id, _objRef, SaveButtonId).ConfigureAwait(true);
            });
        }
    }

    [JSInvokable]
    public async Task OnEnterKey()
    {
        if (SaveEnabled && !string.IsNullOrWhiteSpace(SaveButtonId))
        {
            //await _commonModule.InvokeVoidAsync("focus", SaveButtonId).ConfigureAwait(true);
            if (SaveOnEnter)
            {
                await Task.Delay(100).ConfigureAwait(true);
                await Save.InvokeAsync().ConfigureAwait(true);
            }
        }
    }

    [JSInvokable]
    public async Task OnModalShown()
    {
        StateHasChanged();
        if (!string.IsNullOrWhiteSpace(FocusElementId) && _commonModule != null)
        {
            await Task.Delay(100);
            await _commonModule.InvokeVoidAsync("selectText", FocusElementId).ConfigureAwait(true);
        }
        await Shown.InvokeAsync().ConfigureAwait(true);
    }

    [JSInvokable]
    public async Task OnModalHidden()
    {
        StateHasChanged();
        await Hidden.InvokeAsync().ConfigureAwait(true);
    }

    private async Task OnSave()
    {
        if (SaveEnabled)
        {
            await Save.InvokeAsync().ConfigureAwait(true);
        }
    }

    public async Task ShowAsync()
    {
        await _modal.InvokeVoidAsync("show").ConfigureAwait(true);
    }
}
