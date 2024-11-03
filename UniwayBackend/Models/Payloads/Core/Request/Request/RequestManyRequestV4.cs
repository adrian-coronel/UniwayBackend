namespace UniwayBackend.Models.Payloads.Core.Request.Request
{
    public class RequestManyRequestV4
    {
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public short? TypeCarId { get; set; }
        public int ClientId { get; set; }
        public short AvailabilityId { get; set; }
        public int? ServiceTechnicalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public bool IsResponse { get; set; }

        // Agregar imagenes
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
