using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response.Request;

namespace UniwayBackend.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Aquí puedes realizar alguna lógica adicional si lo necesitas
            await base.OnConnectedAsync();
        }

        // Este metodo puede ser llamado desde 
        public async Task SendNotificationAsync(string UserId, object? obj)
        {

            if (!string.IsNullOrEmpty(UserId))
            {
                // Enviar notificación directamente al usuario autenticado
                await Clients.User(UserId).SendAsync(Constants.TypesMethodsConnection.RECEIVE_NOTIFICATION_REQUESTS, obj);
            }
        }
    }

}
