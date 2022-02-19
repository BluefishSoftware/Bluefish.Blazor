namespace Bluefish.Blazor.Components;

public partial class BfTitleBar
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string IconCssClass { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string TitleCssClass { get; set; }

    /// <summary>
    /// Optional function to check whether user has access to the page.
    /// If returns false then the user will be redirected back to the
    /// AccessFailUrl address.
    /// </summary>
    [Parameter]
    public Func<bool> AccessCondition { get; set; }

    [Parameter]
    public string AccessFailUrl { get; set; } = "/";

    protected override void OnInitialized()
    {
        try
        {
            if(AccessCondition?.Invoke() == false)
            {
                NavigationManager.NavigateTo(String.IsNullOrWhiteSpace(AccessFailUrl) ? "/" : AccessFailUrl);
            }
        }
        catch(NavigationException)
        {
            // save to ignore exeception
        }
    }
}
