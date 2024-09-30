using UniwayBackend.Models.Payloads.Core.Response.Notification;

namespace UniwayBackend.Services.interfaces
{
    public interface INotificationService
    {
        Task SendNotificationWithTechnicalResponseAsync(string UserId, NotificationResponse notification);
        Task SendNotificationWithRequestAsync(string UserId, NotificationResponse notification);
        Task SendSomeNotificationWithRequestAsync(List<string> UsersId, NotificationResponse notification);
    }
}
