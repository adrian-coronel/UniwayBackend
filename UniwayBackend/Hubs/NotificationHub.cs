using Microsoft.AspNetCore.SignalR;
using UniwayBackend.Models.Payloads.Core.Response.Request;

namespace UniwayBackend.Hubs
{
    public class NotificationHub : Hub
    {

        // Mapa para userId a ConnectionId
        private static readonly Dictionary<Guid, string> _userConnections = new(); 

        public override async Task OnConnectedAsync()
        {
            // Este método se llama cuando un cliente se conecta
            await base.OnConnectedAsync();
        }

        public async Task RegisterUser(Guid userId)
        {
            // Asocia el mechanicId con el ConnectionId actual
            var connectionId = Context.ConnectionId;

            // Almacena la relación en un diccionario
            _userConnections[userId] = connectionId;            
        }

        public async Task SendNotificationAsync(string MethodName, Guid userId, string message)
        {
            if (_userConnections.TryGetValue(userId, out var connectionId))
            {
                // Envía la notificación solo al cliente que corresponde al mechanicId
                await Clients.Client(connectionId).SendAsync(MethodName, message);
            }
        }

    }
}
