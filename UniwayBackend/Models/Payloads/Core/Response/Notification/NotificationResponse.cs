using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Models.Payloads.Core.Response.Notification
{
    public class NotificationResponse
    {
        public string Type { get; set; } // "Solicitud", "Estado", etc.
        public string Message { get; set; }
        public object Data { get; set; } // Datos adicionales
        public int? TypeAttentionRequest { get; set; }  //Solicitud urgente o solicitud programada o de servicio.
        public int? StateRequestId { get; set; }
        public string? ImageUser { get; set; }
        public DataUserResponse UserSend { get; set; }
    }
}
