namespace Bluefish.Blazor.Models.ChartJs;

public class Font
{
    /// <summary>
    /// Default font family for all text, follows CSS font-family options.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Family { get; set; } = "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif";

    /// <summary>
    /// Height of an individual line of text.
    /// </summary>
    public double LineHeight { get; set; } = 1.2;

    /// <summary>
    /// Default font size (in px) for text. Does not apply to radialLinear scale point labels.
    /// </summary>
    public int Size { get; set; } = 12;

    /// <summary>
    /// Default font style. Does not apply to tooltip title or footer. Does not apply to chart title.
    /// Follows CSS font-style options (i.e. normal, italic, oblique, initial, inherit).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Style { get; set; } = "normal";

    /// <summary>
    /// Default font weight (boldness).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Weight { get; set; }
}
