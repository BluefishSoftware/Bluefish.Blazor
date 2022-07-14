namespace Bluefish.Blazor.Models.ChartJs;

public class CartesianScale : Scale
{
    /// <summary>
    /// Which type of axis this is. Possible values are: 'x', 'y'. If not set,
    /// this is inferred from the first character of the ID which should be 'x' or 'y'.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Axis { get; set; }

    /// <summary>
    /// Determines the scale bounds.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Bounds { get; set; }

    /// <summary>
    /// If true, extra space is added to the both edges and the axis is scaled to fit into the chart area.
    /// This is set to true for a bar chart by default.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Offset { get; set; }

    /// <summary>
    /// Position of the axis.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Position { get; set; }

    /// <summary>
    /// Scale title configuration.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Title Title { get; set; }
}
