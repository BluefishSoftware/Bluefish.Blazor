namespace Bluefish.Blazor.Components;

public partial class BfTabPanel
{
    private List<BfTabPage> Pages = new List<BfTabPage>();

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; }

    public BfTabPage ActivePage { get; set; }

    internal void AddPage(BfTabPage tabPage)
    {
        Pages.Add(tabPage);
        if (Pages.Count == 1)
            ActivePage = tabPage;
        StateHasChanged();
    }
    private string GetButtonClass(BfTabPage page)
    {
        return page == ActivePage ? "btn-primary" : "btn-secondary";
    }

    private void ActivatePage(BfTabPage page)
    {
        ActivePage = page;
    }
}
