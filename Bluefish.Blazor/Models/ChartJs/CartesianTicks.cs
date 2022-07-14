namespace Bluefish.Blazor.Models.ChartJs;

public class CartesianTicks : Ticks
{
    /// <summary>
    /// If true, automatically calculates how many labels can be shown and hides labels accordingly.
    /// Labels will be rotated up to maxRotation before skipping any.
    /// Turn autoSkip off to show all labels no matter what.
    /// </summary>
    public bool AutoSkip { get; set; } = true;

    /// <summary>
    /// Padding between the ticks on the horizontal axis when autoSkip is enabled.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? AutoSkipPadding { get; set; }

    /// <summary>
    /// Major ticks configuration.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Major { get; set; }

}
