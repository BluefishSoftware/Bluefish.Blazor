namespace Bluefish.Blazor.Models.ChartJs;

public class Options
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Layout Layout { get; set; }

    public bool MaintainAspectRatio { get; set; } = false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Plugins Plugins { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Scales Scales { get; set; }
}
