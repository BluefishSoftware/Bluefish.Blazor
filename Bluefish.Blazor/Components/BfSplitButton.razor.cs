namespace Bluefish.Blazor.Components;

public partial class BfSplitButton
{
    [Parameter]
    public Dictionary<string, object> Attributes { get; set; }

    [Parameter]
    public EventCallback<Option> Click { get; set; }

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public bool IsPrimary { get; set; }

    [Parameter]
    public IEnumerable<Option> Options { get; set; } = Array.Empty<Option>();

    [Parameter]
    public bool PreventDefault { get; set; }

    [Parameter]
    public Sizes Size { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; } = "d-none d-sm-inline-block";

    [Parameter]
    public string Title { get; set; }

    private Dictionary<string, object> ActualAttributes
    {
        get
        {
            var attr = new Dictionary<string, object>(Attributes ?? new())
                {
                    { "disabled", Enabled ? null : true },
                    { "class", $"bf-split-button dropdown-toggle btn {Size.CssClass("btn-sm", "", "btn-lg")} {(IsPrimary ? "btn-primary" : "")} {CssClass}" }
                };
            if (!Visible)
            {
                attr.Add("style", "display: none;");
            }
            return attr;
        }
    }

    private Task OnClickAsync(Option option) => Click.InvokeAsync(option);
}
