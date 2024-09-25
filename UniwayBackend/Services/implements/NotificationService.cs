using Microsoft.AspNetCore.SignalR;
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


        public async Task SendNotificationAsync(Guid UserId, NotificationResponse notification)
        {
            await _hubContext.Clients.User(UserId.ToString())
                .SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_REQUESTS,
                           UserId,
                           notification
                );
        }
    }
}
