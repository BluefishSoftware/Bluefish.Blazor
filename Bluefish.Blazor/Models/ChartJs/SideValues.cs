namespace Bluefish.Blazor.Models.ChartJs;

public class SideValues
{
    /// <summary>
    /// Initializes a new instance of the Padding class.
    /// </summary>
    public SideValues()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Padding class.
    /// </summary>
    /// <param name="x">Padding value to be applied to all sides.</param>
    public SideValues(double x)
    {
        Top = Right = Bottom = Left = x;
    }

    /// <summary>
    /// Initializes a new instance of the Padding class.
    /// </summary>
    /// <param name="x">Padding value to be applied to top and bottom sides.</param>
    /// <param name="y">Padding value to be applied to left and right sides.</param>
    public SideValues(double x, double y)
    {
        Top = Bottom = x;
        Right = Left = y;
    }

    /// <summary>
    /// Initializes a new instance of the Padding class.
    /// </summary>
    /// <param name="top">Padding value to be applied to the top side.</param>
    /// <param name="right">Padding value to be applied to the right side.</param>
    /// <param name="bottom">Padding value to be applied to the bottom side.</param>
    /// <param name="left">Padding value to be applied to the left side.</param>
    public SideValues(double top, double right, double bottom, double left)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }

    /// <summary>
    /// Padding applied to the bottom.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bottom { get; set; }

    /// <summary>
    /// Padding applied to the left.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Left { get; set; }

    /// <summary>
    /// Padding applied to the top.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Top { get; set; }

    /// <summary>
    /// Padding applied to the right.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Right { get; set; }
}
