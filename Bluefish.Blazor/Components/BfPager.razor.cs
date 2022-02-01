namespace Bluefish.Blazor.Components;

public partial class BfPager : IDisposable
{
    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public string NoItemsText { get; set; } = "No items to display";

    [Parameter]
    public PageInfo PageInfo { get; set; } = new PageInfo(1, 10, 0);

    [Parameter]
    public uint[] PageSizes { get; set; } = new uint[] { 10, 25, 50, 100, 250, 500 };

    [Parameter]
    public bool ShowPageDescription { get; set; } = true;

    [Parameter]
    public bool ShowPageSizes { get; set; } = true;


    [Parameter]
    public Sizes Size { get; set; }

    protected override void OnInitialized()
    {
        if (PageInfo != null)
        {
            PageInfo.Changed += PageCriteria_Changed;
            PageInfo.TotalCountChanged += PageInfo_TotalCountChanged;
        }
    }

    private void PageInfo_TotalCountChanged(object sender, EventArgs e)
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        if (PageInfo != null)
        {
            PageInfo.Changed -= PageCriteria_Changed;
            PageInfo.TotalCountChanged -= PageInfo_TotalCountChanged;
        }
        GC.SuppressFinalize(this);
    }

    private void PageCriteria_Changed(object sender, System.EventArgs e)
    {
        StateHasChanged();
    }

    public void MoveLast()
    {
        PageInfo.Page = PageInfo.PageCount;
    }

    public void MoveNext()
    {
        PageInfo.Page++;
    }

    public void MovePrevious()
    {
        PageInfo.Page--;
    }

    public void MoveFirst()
    {
        PageInfo.Page = 1;
    }
}
