namespace Bluefish.Blazor.Models.ChartJs;

public class ArcDataset : Dataset
{
    /// <summary>
    /// Arc offset (in pixels).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Offset { get; set; }

    /// <summary>
    /// Fixed arc offset (in pixels). Similar to offset but applies to all arcs.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Spacing { get; set; }
}
