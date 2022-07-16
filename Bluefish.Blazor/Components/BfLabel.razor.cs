namespace Bluefish.Blazor.Components;

public partial class BfLabel
{
    [Parameter]
    public EventCallback<MouseEventArgs> Click { get; set; }

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public bool PreventDefault { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; }


    [Parameter]
    public string Title { get; set; }

    protected override Dictionary<string, object> RootAttributes
    {
        get
        {
            var attr = base.RootAttributes;
            if (!attr.ContainsKey("class"))
            {
                attr.Add("class", $"bf-label {(Enabled ? "" : "disabled")} {(string.IsNullOrEmpty(CssClass) ? "text-nowrap" : CssClass)}");
            }
            return attr;
        }
    }
}
