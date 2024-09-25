﻿using UniwayBackend.Models.Payloads.Core.Response.Notification;

namespace UniwayBackend.Services.interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid UserId, NotificationResponse message);
    }
}
