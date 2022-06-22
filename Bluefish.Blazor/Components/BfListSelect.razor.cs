namespace Bluefish.Blazor.Components;

public partial class BfListSelect<TKey>
{
    [Parameter]
    public IEnumerable<KeyValuePair<TKey, string>> Options { get; set; } = Array.Empty<KeyValuePair<TKey, string>>();

    [Parameter]
    public bool MultipleSelect { get; set; } = true;

    [Parameter]
    public TKey[] Value { get; set; } = Array.Empty<TKey>();

    [Parameter]
    public EventCallback<TKey[]> ValueChanged { get; set; }

    private async Task OnOptionInput(TKey key, bool isChecked)
    {
        var currentSelection = new List<TKey>(Value);
        if (isChecked && !currentSelection.Contains(key))
        {
            if (!MultipleSelect)
            {
                currentSelection.Clear();
            }
            currentSelection.Add(key);
        }
        else if (!isChecked && currentSelection.Contains(key))
        {
            currentSelection.Remove(key);
        }
        Value = currentSelection.ToArray();
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(true);
    }
}

