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


        public async Task SendNotificationWithRequestAsync(string UserId, NotificationResponse notification)
        {

            await _hubContext.Clients.User(UserId)
                .SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_REQUESTS,
                           JsonSerializer.Serialize(notification));
        }


        public async Task SendSomeNotificationWithRequestAsync(List<string> UsersId, NotificationResponse notification)
        {
            await _hubContext.Clients.Users(UsersId)
                .SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_REQUESTS,
                           JsonSerializer.Serialize(notification));
        }

        public async Task SendNotificationWithTechnicalResponseAsync(string UserId, NotificationResponse notification)
        {
            await _hubContext.Clients.User(UserId)
                .SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_TECH_RESP,
                           JsonSerializer.Serialize(notification));
        }

        public async Task SendSomeNotificationChangeStateRequestAsync(List<string> UsersId, NotificationResponse notification)
        {
            await _hubContext.Clients.Users(UsersId)
                .SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_CHANGE_STATE_REQUEST,
                           JsonSerializer.Serialize(notification));
        }

        public async Task SendNotificationChangeStateRequestAsync(string UserId, NotificationResponse notification)
        {
            await _hubContext.Clients.User(UserId)
                .SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_CHANGE_STATE_REQUEST,
                           JsonSerializer.Serialize(notification));
        }
    }
}
