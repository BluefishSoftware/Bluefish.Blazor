using Microsoft.AspNetCore.Components;

namespace Bluefish.Blazor.Interfaces
{
    public interface INotificationService
    {
        void Notify(NotificationLevels level, string message, string title = "");
        void Notify(NotificationLevels level, RenderFragment message, string title = "");
    }

    public enum NotificationLevels
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3
    }
}
