using UniwayBackend.Models.Payloads.Core.Response.ImagesServiceTechnical;

namespace UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical
{
    public class ServiceTechnicalResponse
    {
        public int Id { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }
        public short CategoryServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<ImagesServiceTechnicalResponse> Images { get; set; }
    }
}
