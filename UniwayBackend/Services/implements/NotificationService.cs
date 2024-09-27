using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;
using UniwayBackend.Config;
using UniwayBackend.Hubs;
using UniwayBackend.Models.Payloads.Core.Response.Notification;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class NotificationService : INotificationService
    {

        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task SendNotificationAsync(string UserId, NotificationResponse notification)
        {

            await _hubContext.Clients.User(UserId)
                .SendAsync("ReceiveNotificationRequests",
                           JsonSerializer.Serialize(notification));
        }
    }
}
