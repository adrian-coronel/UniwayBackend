

using UniwayBackend.Models.Payloads.Core.Response.Material;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;

namespace UniwayBackend.Models.Payloads.Core.Response.TechnicalResponse
{
    public class TechnicalResponseResponseV2
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public TechnicalProfessionAvailabilityResponse? TechnicalProfessionAvailability { get;set; }
        public int? WorkshopTechnicalProfessionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Relations
        public virtual List<MaterialResponse> Materials { get; set; }
    }
}
