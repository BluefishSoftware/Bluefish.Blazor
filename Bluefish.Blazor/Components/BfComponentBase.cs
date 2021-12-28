namespace Bluefish.Blazor.Components;

public abstract class BfComponentBase : ComponentBase
{
    private static int _sequence;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

    [Parameter]
    public string Id { get; set; } = $"bf-component-{++_sequence}";

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public bool Enabled { get; set; } = true;

    [Parameter]
    public bool Visible { get; set; } = true;

    protected virtual Dictionary<string, object> RootAttributes
    {
        get
        {
            var attr = new Dictionary<string, object>(AdditionalAttributes ?? new Dictionary<string, object>());
            if (!Enabled)
            {
                attr.Add("disabled", true);
            }
            if (!attr.ContainsKey("id"))
            {
                attr.Add("id", Id);
            }
            if (!Visible)
            {
                attr.Add("style", "display: none;");
            }
            return attr;
        }
    }
}
