namespace Bluefish.Blazor.Models.ChartJs;

public class Layout
{
    public bool AutoPadding { get; set; } = true;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Padding { get; set; }
}
