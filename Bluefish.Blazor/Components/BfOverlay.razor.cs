namespace Bluefish.Blazor.Components;

public partial class BfOverlay : IDisposable
{
    private bool _visible;
    private string _html;

    [Inject] protected IOverlayService OverlayService { get; set; } = null!;

    protected override void OnInitialized()
    {
        OverlayService.Shown += OverlayService_Shown;
        OverlayService.Hidden += OverlayService_Hidden;
    }

    private void OverlayService_Hidden()
    {
        _visible = false;
        StateHasChanged();
    }

    private void OverlayService_Shown(string html)
    {
        _html = html;
        _visible = true;
        StateHasChanged();
    }

    public void Dispose()
    {
        OverlayService.Shown -= OverlayService_Shown;
        OverlayService.Hidden -= OverlayService_Hidden;
        GC.SuppressFinalize(this);
    }
}
