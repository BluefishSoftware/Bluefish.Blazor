namespace Bluefish.Blazor.Models;

public class ToolbarItemBase : IToolbarItem
{
    public string CssClass { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public string IconCssClass { get; set; } = string.Empty;
    public bool PositionRight { get; set; }
    public string Text { get; set; } = string.Empty;
    public string TextCssClass { get; set; } = string.Empty;
    public bool Visible { get; set; } = true;
}
