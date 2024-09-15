using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;

namespace UniwayBackend.Models.Payloads.Core.Response.CategoryService
{
    public class CategoryServiceResponse
    {
        public short Id { get; set; }
        public string Name { get; set; }

        public List<ServiceTechnicalResponse> ServiceTechnicals { get; set; } = new List<ServiceTechnicalResponse>();
    }
}
