namespace Bluefish.Blazor.Components;

public partial class BfButton
{
    [Parameter]
    public Dictionary<string, object> Attributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> Click { get; set; }

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public bool IsPrimary { get; set; }

    [Parameter]
    public bool IsSubmit { get; set; }

    [Parameter]
    public bool PreventDefault { get; set; }

    [Parameter]
    public Sizes Size { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; } = "d-none d-lg-inline-block";

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Url { get; set; } = string.Empty;

    [Parameter]
    public string UrlTarget { get; set; } = string.Empty;

    private Dictionary<string, object> ActualAttributes
    {
        get
        {
            var attr = new Dictionary<string, object>(Attributes ?? new())
                {
                    { "disabled", Enabled ? null : true },
                    { "class", $"bf-button btn {Size.CssClass("btn-sm", "", "btn-lg")} {(IsPrimary ? "btn-primary" : "")} {CssClass}" }
                };
            if (!Visible)
            {
                attr.Add("style", "display: none;");
            }
            return attr;
        }
    }
}
