using Bluefish.Blazor.Interfaces;
using System;

namespace Bluefish.Blazor.Services
{
    public class OverlayService : IOverlayService
    {
        public event Action Hidden;

        public event Action<string> Shown;

        public void Hide()
        {
            Hidden?.Invoke();
        }

        public void Show(string html = null)
        {
            Shown?.Invoke(html);
        }
    }
}
