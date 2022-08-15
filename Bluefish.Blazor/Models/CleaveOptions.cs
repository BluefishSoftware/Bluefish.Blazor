namespace Bluefish.Blazor.Models;

public class CleaveOptions
{
    public int[] Blocks { get; set; } = Array.Empty<int>();

    public string Delimiter { get; set; } = " ";

    public string[] Delimiters { get; set; } = Array.Empty<string>();

    public bool Numeral { get; set; }

    public int NumeralDecimalScale { get; set; } = 2;

    public string NumeralThousandsGroupStyle { get; set; } = "thousand";
}