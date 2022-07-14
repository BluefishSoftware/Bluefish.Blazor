namespace Bluefish.Blazor.Models.ChartJs;

public class LineDataset : Dataset
{
    /// <summary>
    /// The radius of the point shape. If set to 0, the point is not rendered.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? PointRadius { get; set; }

    /// <summary>
    /// Style of the point.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string PointStyle { get; set; }

    /// <summary>
    /// If false, the line is not drawn for this dataset.
    /// </summary>
    public bool ShowLine { get; set; } = true;

    /// <summary>
    /// The following values are supported for stepped.
    /// false: No Step Interpolation (default), true: Step-before Interpolation(eq. 'before'), 'before': Step-before Interpolation
    /// 'after': Step-after Interpolation and 'middle': Step-middle Interpolation
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Stepped { get; set; }
}
