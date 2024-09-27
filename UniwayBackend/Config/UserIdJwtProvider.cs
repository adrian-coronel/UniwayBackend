using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace UniwayBackend.Config
{
    public class UserIdJwtProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("id")?.Value;
        }
    }
}
