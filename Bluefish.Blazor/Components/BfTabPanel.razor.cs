namespace Bluefish.Blazor.Components;

public partial class BfTabPanel
{
    private List<BfTabPage> Pages = new List<BfTabPage>();

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public Sizes Size { get; set; }

    public BfTabPage ActivePage { get; set; }

    internal void AddPage(BfTabPage tabPage)
    {
        Pages.Add(tabPage);
        if (Pages.Count == 1)
            ActivePage = tabPage;
        StateHasChanged();
    }
    private string GetButtonClass(BfTabPage page)
        => $"{(page == ActivePage ? "btn-primary" : "btn-secondary")} {Size.CssClass("btn-sm", "", "btn-lg")}";

    private void ActivatePage(BfTabPage page)
    {
        ActivePage = page;
    }
}
