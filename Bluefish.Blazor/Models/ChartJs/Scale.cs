namespace Bluefish.Blazor.Models.ChartJs;
public class Scale
{
    /// <summary>
    /// Align pixel values to device pixels.
    /// </summary>
    public bool AlignToPixels { get; set; }

    /// <summary>
    /// Background color of the scale area.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string BackgroundColor { get; set; }

    /// <summary>
    /// Controls the axis global visibility (visible when true, hidden when false).
    /// When display: 'auto', the axis is visible only if at least one associated dataset is visible.
    /// </summary>
    public bool Display { get; set; } = true;

    /// <summary>
    /// Grid line configuration.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Grid Grid { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[] Labels { get; set; }

    /// <summary>
    /// User defined maximum number for the scale, overrides maximum value from data.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Max { get; set; }

    /// <summary>
    /// User defined minimum number for the scale, overrides minimum value from data.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Min { get; set; }

    /// <summary>
    /// Reverse the scale.
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// Should the data be stacked.
    /// </summary>
    public bool Stacked { get; set; }

    /// <summary>
    /// Adjustment used when calculating the maximum data value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? SuggestedMax { get; set; }

    /// <summary>
    /// Adjustment used when calculating the minimum data value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? SuggestedMin { get; set; }

    /// <summary>
    /// Tick configuration.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Ticks { get; set; }

    /// <summary>
    /// Type of scale being employed. Custom scales can be created and registered with a string key.
    /// This allows changing the type of an axis for a chart.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Type { get; set; }

    /// <summary>
    /// The weight used to sort the axis. Higher weights are further away from the chart area.
    /// </summary>
    public double Weight { get; set; }
}
