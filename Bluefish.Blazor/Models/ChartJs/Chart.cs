namespace Bluefish.Blazor.Models.ChartJs;

public class Chart
{
    public string Type { get; set; } = "bar";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Data Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Options Options { get; set; }

    public override string ToString()
    {
        return $"Type: {Type}, Labels: {string.Join(", ", Data.Labels)}";
    }
}
