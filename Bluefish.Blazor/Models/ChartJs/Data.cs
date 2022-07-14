namespace Bluefish.Blazor.Models.ChartJs;

public class Data
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[] Labels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object[] Datasets { get; set; }
}
