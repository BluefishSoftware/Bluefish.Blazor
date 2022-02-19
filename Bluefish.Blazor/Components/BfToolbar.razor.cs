namespace Bluefish.Blazor.Components;

public partial class BfToolbar
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public IEnumerable<IToolbarItem> Items { get; set; } = Array.Empty<IToolbarItem>();

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private async Task OnButtonClick(ToolbarButton button, MouseEventArgs args)
    {
        if(button.ClickAsync != null)
        {
            await button.ClickAsync.Invoke().ConfigureAwait(true);
        }
        else if (button.Click != null)
        {
            button.Click.Invoke();
        }
    }
}
