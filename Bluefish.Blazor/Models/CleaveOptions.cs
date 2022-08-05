namespace Bluefish.Blazor.Models;

public class CleaveOptions
{
    public bool Numeral { get; set; }
    public string NumeralThousandsGroupStyle { get; set; } = "thousand";
    public int NumeralDecimalScale { get; set; } = 2;
}