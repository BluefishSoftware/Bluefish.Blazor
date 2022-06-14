namespace Bluefish.Blazor.Models;

public class EditOptions
{
    public string Format { get; set; } = string.Empty;

    public bool IsEditable { get; set; } = true;

    public bool Required { get; set; }
}
