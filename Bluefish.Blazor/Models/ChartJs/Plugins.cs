namespace Bluefish.Blazor.Models.ChartJs;

public class Plugins
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Legend Legend { get; set; }
}
