namespace Bluefish.Blazor.Models;

/// <summary>
/// The SortInfo class contains sort details.
/// </summary>
public class SortInfo
{
    /// <summary>
    /// Gets or sets the unique identifier of the column to be sorted.
    /// </summary>
    public string ColumnId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the direction of the sort.
    /// </summary>
    public SortDirections Direction { get; set; }
}
