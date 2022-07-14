namespace Bluefish.Blazor.Models.ChartJs;

public class Title
{
    public string Align { get; set; } = "center";
    public string Color { get; set; }
    public bool Display { get; set; } = true;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Font Font { get; set; }

    public int Padding { get; set; } = 4;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Text { get; set; }
}
