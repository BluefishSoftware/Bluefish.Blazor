namespace Bluefish.Blazor.Models;

public class EditOptions
{
    public int DecimalPlaces { get; set; } = 2;

    public string Format { get; set; } = string.Empty;

    public bool IsEditable { get; set; }

    public bool IsNumber { get; set; } = true;

    public bool Required { get; set; }

    public bool SelectAllOnEdit { get; set; }
}
