namespace UniwayBackend.Models.Payloads.Core.Request.ServiceTechnical
{
    public class ServiceTechnicalRequest
    {
        public int TechnicalProfessionAvailabilityId { get; set; }
        public short CategoryServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
