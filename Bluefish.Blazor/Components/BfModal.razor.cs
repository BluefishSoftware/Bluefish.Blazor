﻿namespace Bluefish.Blazor.Components;

public partial class BfModal : IAsyncDisposable
{
    private static int _seq;
    private DotNetObjectReference<BfModal> _objRef;
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
    public RenderFragment Footer { get; set; }

    [Parameter]
    public RenderFragment Header { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-modal-{++_seq}";

    [Parameter]
    public EventCallback Save { get; set; }

    [Parameter]
    public bool SaveEnabled { get; set; } = true;

    [Parameter]
    public string SaveText { get; set; } = "Save";

    [Parameter]
    public bool ShowCloseButton { get; set; }

    [Parameter]
    public bool ShowSave { get; set; } = true;

    [Parameter]
    public Sizes Size { get; set; } = Sizes.Medium;

    [Parameter]
    public string Title { get; set; }

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

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _objRef = DotNetObjectReference.Create(this);
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/_content/Bluefish.Blazor/js/interop.js").ConfigureAwait(true);
            _modal = await _module.InvokeAsync<IJSObjectReference>("initModal", Id, _objRef).ConfigureAwait(true);
        }
    }

    [JSInvokable]
    public void OnModalShown()
    {
        StateHasChanged();
    }

    [JSInvokable]
    public void OnModalHidden()
    {
        StateHasChanged();
    }

    public async Task ShowAsync()
    {
        await _modal.InvokeVoidAsync("show").ConfigureAwait(true);
    }
}
