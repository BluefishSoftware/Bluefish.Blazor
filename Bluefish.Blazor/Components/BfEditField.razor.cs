namespace Bluefish.Blazor.Components;

public partial class BfEditField<TValue> : IAsyncDisposable where TValue : IConvertible
{
    private static int _sequence;
    private bool _isEditing;
    private IJSObjectReference _module;
    private IJSObjectReference _commonModule;
    private DotNetObjectReference<BfEditField<TValue>> _objRef;
    //private PersistingComponentStateSubscription _stateSubscription;

    #region Inject

    [Inject]
    public PersistentComponentState ApplicationState { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    #endregion

    [Parameter]
    public string Id { get; set; } = $"edit-field-{System.Threading.Interlocked.Increment(ref _sequence)}";

    [Parameter]
    public EditOptions Options { get; set; } = new();

    [Parameter]
    public TValue Value { get; set; } = default;

    [Parameter]
    public EventCallback<TValue> ValueChanged { get; set; }

    private string EditorId => $"{Id}-editor";
    private string LabelId => $"{Id}-label";

    public async Task BeginEditAsync()
    {
        if (!_isEditing && Options.IsEditable)
        {
            _isEditing = true;
            if (_commonModule != null)
            {
                await _commonModule.InvokeVoidAsync("addClass", LabelId, "d-none").ConfigureAwait(true);
                await _commonModule.InvokeVoidAsync("setValue", EditorId, Value).ConfigureAwait(true);
                await _commonModule.InvokeVoidAsync("removeClass", EditorId, "d-none").ConfigureAwait(true);
                if (Options.SelectAllOnEdit)
                {
                    await _commonModule.InvokeVoidAsync("selectText", EditorId).ConfigureAwait(true);
                }
                else
                {
                    await _commonModule.InvokeVoidAsync("focus", EditorId).ConfigureAwait(true);
                }
            }
        }
    }

    private TValue ConvertValue(string value)
    {
        return (TValue)Convert.ChangeType(value, typeof(TValue));
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            GC.SuppressFinalize(this);
            //_stateSubscription.Dispose();
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
                if (_commonModule != null)
                {
                    try
                    {
                        // get users value and convert to type
                        var textValue = await _commonModule.InvokeAsync<string>("getValue", EditorId).ConfigureAwait(true);
                        var editValue = ConvertValue(textValue);

                        // has value changed
                        if (!editValue.Equals(Value))
                        {
                            // update value only if valid
                            if (Validate(editValue))
                            {
                                Value = editValue;
                                await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
                            }
                        }
                    }
                    catch
                    {
                        // conversion error - value is invalid
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
                await _module.InvokeVoidAsync("initialize", EditorId, _objRef, new CleaveOptions
                {
                    Numeral = Options.IsNumber,
                    NumeralDecimalScale = Options.DecimalPlaces
                }).ConfigureAwait(true);
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

    private bool Validate(TValue value)
    {
        if (Options.Required && string.IsNullOrWhiteSpace(value?.ToString()))
        {
            return false;
        }

        return true;
    }
}