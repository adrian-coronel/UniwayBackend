using UniwayBackend.Models.Payloads.Core.Request.Material;

namespace UniwayBackend.Models.Payloads.Core.Request.TechnicalResponse
{
    public class TechnicalResponseRequest
    {
        public int RequestId { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public int? WorkshopTechnicalProfessionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime? PropossedAssistanceDate { get; set; }

        // Relations
        public virtual List<MaterialRequest> Materials { get; set; }
    }
}
