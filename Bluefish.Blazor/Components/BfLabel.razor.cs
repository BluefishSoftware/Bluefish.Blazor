namespace Bluefish.Blazor.Components;

public partial class BfLabel
{
    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string TextCssClass { get; set; }

    protected override Dictionary<string, object> RootAttributes
    {
        get
        {
            var attr = base.RootAttributes;
            if (!attr.ContainsKey("class"))
            {
                attr.Add("class", $"bf-label {(Enabled ? "" : "disabled")}");
            }
            return attr;
        }
    }
}
