using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Request.Request
{
    public class RequestManyRequestV5
    {
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public short TypeCarId { get; set; }
        public int ClientId { get; set; }
        public short AvailabilityId { get; set; }
        public int? ServiceTechnicalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        [Required(ErrorMessage = "El campo de FromShow es obligatorio")]
        public DateTime FromShow { get; set; }
        [Required(ErrorMessage = "El campo de ToShow es obligatorio")]
        public DateTime ToShow { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public bool IsResponse { get; set; }

        // Agregar imagenes
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
