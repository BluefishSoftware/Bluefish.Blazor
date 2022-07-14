namespace Bluefish.Blazor.Models.ChartJs;

public class Scales
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object X { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Y { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Y2 { get; set; }
}
