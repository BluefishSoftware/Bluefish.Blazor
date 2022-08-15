namespace Bluefish.Blazor.Models;

public class TableEditInfo<TKey>
{
    public TKey ItemId { get; set; }

    public string ColumnId { get; set; }

    public object NewValue { get; set; }

    public object OldValue { get; set; }
}
