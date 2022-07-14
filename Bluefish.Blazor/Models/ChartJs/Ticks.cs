namespace Bluefish.Blazor.Models.ChartJs;

public class Ticks
{
    /// <summary>
    /// Color of label backdrops.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string BackdropColor { get; set; }

    /// <summary>
    /// Padding of label backdrop.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SideValues BackdropPadding { get; set; }

    /// <summary>
    /// If true, show tick labels.
    /// </summary>
    public bool Display { get; set; } = true;

    /// <summary>
    /// Color of ticks.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Color { get; set; }

    /// <summary>
    /// Font for tick labels.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Font Font { get; set; }

    /// <summary>
    /// Custom field that specifies Numerical string format for tick labels.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Format { get; set; }

    /// <summary>
    /// Sets the offset of the tick labels from the axis.
    /// </summary>
    public double Padding { get; set; }

    /// <summary>
    /// If true, draw a background behind the tick labels.
    /// </summary>
    public bool ShowLabelBackdrop { get; set; }

    /// <summary>
    /// The color of the stroke around the text.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string TextStrokeColor { get; set; }

    /// <summary>
    /// Stroke width around the text.
    /// </summary>
    public double TextStrokeWidth { get; set; }

    /// <summary>
    /// z-index of tick layer. Useful when ticks are drawn on chart area. Values <= 0 are drawn under datasets, > 0 on top.
    /// </summary>
    public double Z { get; set; }
}
