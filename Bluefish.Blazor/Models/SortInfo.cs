namespace Bluefish.Blazor.Models;

/// <summary>
/// The SortInfo class contains sort details.
/// </summary>
public class SortInfo
{
    /// <summary>
    /// Gets or sets the name of the field the sort operation is performed upon.
    /// </summary>
    public string DataField { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the direction of the sort.
    /// </summary>
    public SortDirections Direction { get; set; }
}
