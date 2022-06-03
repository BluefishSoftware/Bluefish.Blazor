namespace Bluefish.Blazor.Components;

public partial class BfEditField : IAsyncDisposable
{
    private static int _sequence;
    private bool _isEditing;
    private IJSObjectReference _module;
    private IJSObjectReference _commonModule;
    private DotNetObjectReference<BfEditField> _objRef;

    #region Inject

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    #endregion

    [Parameter]
    public string Id { get; set; } = $"edit-field-{++_sequence}";

    [Parameter]
    public EditOptions Options { get; set; } = new();

    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    private string EditorId => $"{Id}-editor";
    private string LabelId => $"{Id}-label";

    public async Task BeginEditAsync()
    {
        if (Options.IsEditable && !_isEditing)
        {
            _isEditing = true;
            if (_commonModule != null)
            {
                await _commonModule.InvokeVoidAsync("addClass", LabelId, "d-none").ConfigureAwait(true);
                await _commonModule.InvokeVoidAsync("setValue", EditorId, Value).ConfigureAwait(true);
                await _commonModule.InvokeVoidAsync("removeClass", EditorId, "d-none").ConfigureAwait(true);
                await _commonModule.InvokeVoidAsync("selectText", EditorId).ConfigureAwait(true);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            if (_module != null)
            {
                await _module.InvokeVoidAsync("dispose", Id).ConfigureAwait(true);
                await _module.DisposeAsync().ConfigureAwait(false);
            }
            if (_commonModule != null)
            {
                await _commonModule.DisposeAsync().ConfigureAwait(false);
            }
        }
        catch
        {
        }
    }

    public async Task EndEditAsync(bool cancelEdit = false)
    {
        if (_isEditing)
        {
            _isEditing = false;

            if (!cancelEdit)
            {
                // has value changed?
                var editValue = Value;
                if (_commonModule != null)
                {
                    editValue = await _commonModule.InvokeAsync<string>("getValue", EditorId).ConfigureAwait(true);
                }
                if (editValue != Value)
                {
                    // update value only if valid
                    if (Validate(editValue))
                    {
                        Value = editValue;
                        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
                    }
                }
            }

            // set back to readonly
            if (_commonModule != null)
            {
                await _commonModule.InvokeVoidAsync("addClass", EditorId, "d-none").ConfigureAwait(true);
                await _commonModule.InvokeVoidAsync("removeClass", LabelId, "d-none").ConfigureAwait(true);
            }

            StateHasChanged();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && JSRuntime != null)
        {
            // listen for esc and enter key presses
            _objRef = DotNetObjectReference.Create(this);
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/Components/BfEditField.razor.js").ConfigureAwait(true);
            _commonModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Bluefish.Blazor/js/common.js").ConfigureAwait(true);
            if (_module != null)
            {
                await _module.InvokeVoidAsync("initialize", Id, _objRef).ConfigureAwait(true);
            }
        }
    }
    private async Task OnClick()
    {
        if (Options.IsEditable)
        {
            await BeginEditAsync().ConfigureAwait(true);
        }
    }

    [JSInvokable("OnEscapeKey")]
    public Task OnEscapeKeyAsync()
    {
        return EndEditAsync(true);
    }

    [JSInvokable("OnEnterKey")]
    public Task OnEnterKeyAsync()
    {
        return EndEditAsync();
    }

    private void OnInputBlur()
    {
        _ = EndEditAsync();
    }


    private bool Validate(string value)
    {
        if (Options.Required && string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return true;
    }
}