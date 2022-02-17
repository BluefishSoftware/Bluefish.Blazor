namespace Bluefish.Blazor.Models;

public class Option
{
    public string Text { get; set; }
    public bool Enabled { get; set; } = true;
    public string ToolTip { get; set; }
    public string Value { get; set; }
}
