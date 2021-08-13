﻿using Blazored.Toast.Services;
using Bluefish.Blazor.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bluefish.Blazor.Services
{
    public class ToastNotificationService : INotificationService
    {
        private readonly IToastService _toastService;

        public ToastNotificationService(IToastService toastService)
        {
            _toastService = toastService;
        }

        public void Notify(NotificationLevels level, string message, string title = "")
        {
            _toastService.ShowToast((ToastLevel)level, message, title);
        }

        public void Notify(NotificationLevels level, RenderFragment message, string title = "")
        {
            _toastService.ShowToast((ToastLevel)level, message, title);
        }
    }
}
