namespace Bluefish.Blazor.Models;

public class Option
{
    public Option()
    {
    }

    public Option(string text)
    {
        Text = text;
    }

    public Option(string text, string value)
    {
        Text = text;
        Value = value;
    }

    public Option(string text, string value, string toolTip)
    {
        Text = text;
        Value = value;
        ToolTip = ToolTip;
    }

    public string Text { get; set; }
    public bool Enabled { get; set; } = true;
    public string ToolTip { get; set; }
    public string Value { get; set; }
}
