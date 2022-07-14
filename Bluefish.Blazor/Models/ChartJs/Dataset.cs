namespace Bluefish.Blazor.Models.ChartJs;

public class Dataset
{
    /// <summary>
    /// The bar background color.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object BackgroundColor { get; set; }

    /// <summary>
    /// Percent (0-1) of the available width each bar should be within the category width.
    /// 1.0 will take the whole category width and put the bars right next to each other.
    /// </summary>
    public double BarPercentage { get; set; } = 0.9;

    /// <summary>
    /// If this value is a number, it is applied to the width of each bar, in pixels. When this is enforced,
    /// barPercentage and categoryPercentage are ignored.
    /// If set to 'flex', the base sample widths are calculated automatically based on the previous and following samples
    /// so that they take the full available widths without overlap. Then, bars are sized using barPercentage and categoryPercentage.
    /// There is no gap when the percentage options are 1. This mode generates bars with different widths when data are not evenly spaced.
    /// If not set (default), the base sample widths are calculated using the smallest interval that prevents bar overlapping,
    /// and bars are sized using barPercentage and categoryPercentage. This mode always generates bars equally sized.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string BarThickness { get; set; }

    /// <summary>
    /// The bar border color.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object BorderColor { get; set; }

    /// <summary>
    /// This setting is used to avoid drawing the bar stroke at the base of the fill, or disable the border radius. In general,
    /// this does not need to be changed except when creating chart types that derive from a bar chart.
    /// </summary>
    /// <remarks> Options are: 'start','end','bottom','left','top','right',false</remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object BorderSkipped { get; set; }

    /// <summary>
    /// If this value is a number, it is applied to all sides of the rectangle (left, top, right, bottom), except borderSkipped.
    /// If this value is an object, the left property defines the left border width. Similarly, the right, top, and bottom properties
    /// can also be specified. Omitted borders and borderSkipped are skipped.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object BorderWidth { get; set; }

    /// <summary>
    /// If this value is a number, it is applied to all corners of the rectangle (topLeft, topRight, bottomLeft, bottomRight),
    /// except corners touching the borderSkipped. If this value is an object, the topLeft property defines the top-left corners border radius.
    /// Similarly, the topRight, bottomLeft, and bottomRight properties can also be specified. Omitted corners and those touching
    /// the borderSkipped are skipped. For example if the top border is skipped, the border radius for the corners topLeft and topRight
    /// will be skipped as well.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object BorderRadius { get; set; }

    /// <summary>
    /// Percent (0-1) of the available width each category should be within the sample width.
    /// </summary>
    public double CategoryPercentage { get; set; } = 0.8;

    /// <summary>
    /// How to clip relative to chartArea. Positive value allows overflow, negative value clips that many pixels inside chartArea.
    /// 0 = clip at chartArea. Clipping can also be configured per side: clip: {left: 5, top: false, right: -2, bottom: 0}
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Clip { get; set; }

    /// <summary>
    /// All of the supported data structures can be used with bar charts.
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    /// Custom field that specifies Numerical string format for data set values.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Format { get; set; }

    /// <summary>
    /// Should the bars be grouped on index axis. When true, all the datasets at same index value will be placed next to each other
    /// centering on that index value. When false, each bar is placed on its actual index-axis value.
    /// </summary>
    public bool Grouped { get; set; } = true;

    /// <summary>
    /// The base axis of the dataset. 'x' for horizontal lines and 'y' for vertical lines.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string IndexAxis { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Label { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Type { get; set; }

    /// <summary>
    /// The ID of the x-axis to plot this dataset on.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string XAxisID { get; set; }

    /// <summary>
    /// The ID of the y-axis to plot this dataset on.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string YAxisID { get; set; }
}
