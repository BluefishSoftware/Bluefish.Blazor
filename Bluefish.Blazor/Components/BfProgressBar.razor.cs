namespace Bluefish.Blazor.Components;

public partial class BfProgressBar
{
    [Parameter]
    public RenderFragment<BfProgressBar> BarContent { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public ushort DecimalPlaces { get; set; }

    [Parameter]
    public string Height { get; set; } = "100%";

    [Parameter]
    public double Total { get; set; } = 100;

    [Parameter]
    public double Value { get; set; }

    public double GetPercentage()
    {
        if (Total == 0)
        {
            return 0;
        }
        else
        {
            return Math.Round((Value / Total) * 100, DecimalPlaces);
        }
    }
}
