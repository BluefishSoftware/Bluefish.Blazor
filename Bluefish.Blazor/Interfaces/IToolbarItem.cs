namespace Bluefish.Blazor.Interfaces;

public interface IToolbarItem
{
    public string CssClass { get; set; }
    public bool Enabled { get; set; }
    public string IconCssClass { get; set; }
    public bool PositionRight { get; set; }
    public string Text { get; set; }
    public string TextCssClass { get; set; }
    public bool Visible { get; set; }

}
