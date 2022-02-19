namespace Bluefish.Blazor.Models;

public class ToolbarButton : ToolbarItemBase
{
    public ToolbarButton()
    {
    }

    public ToolbarButton(string text, string iconCssClass = null, bool enabled = true, string cssClass = null)
    {
        CssClass = cssClass;
        Enabled = enabled;
        IconCssClass = iconCssClass;
        Text = text;
    }

    public Action Click { get; set; }

    public Func<Task> ClickAsync { get; set; }

    public string Url { get; set; } = string.Empty;

    public string UrlTarget { get; set; } = string.Empty;
}
