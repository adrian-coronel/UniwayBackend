using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Models.Payloads.Core.Response.Notification
{
    public class NotificationResponse
    {
        public string Type { get; set; } // "Solicitud", "Estado", etc.
        public string Message { get; set; }
        public object Data { get; set; } // Datos adicionales
        public DataUserResponse UserSend { get; set; }
    }
}
