namespace Bluefish.Blazor.Interfaces;

public interface IOverlayService
{
    /// <summary>
    /// This is the action to execute when Hide is called
    /// </summary>
    event Action Hidden;

    /// <summary>
    /// This is the action to execute when Show is called
    /// </summary>
    event Action<string> Shown;

    /// <summary>
    /// Hide the overlay.
    /// </summary>
    void Hide();

    /// <summary>
    /// Display the overlay.
    /// </summary>
    /// <param name="html">The HTML to include</param>
    void Show(string html = null);
}
