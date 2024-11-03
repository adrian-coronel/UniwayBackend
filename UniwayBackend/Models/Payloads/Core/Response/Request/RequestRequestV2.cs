namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestRequestV2
    {
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public int ClientId { get; set; }
        public int? ServiceTechnicalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public bool IsResponse { get; set; }
        public DateTime? CreatedOn { get; set; }

        // Lista de mecánicos

        // Agregar imagenes
        public List<IFormFile> Files { get; set; }
    }
}
