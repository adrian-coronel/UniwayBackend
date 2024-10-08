using NetTopologySuite.Geometries;

namespace UniwayBackend.Models.Entities
{
    public class UserRequest
    {
        public Guid UserId { get; set; }
        public int RequestId { get; set; }
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public int ClientId { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public int? ServiceTechnicalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Point Location { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public DateTime? FromShow { get; set; }
        public DateTime? ToShow { get; set; }
        public bool IsResponse { get; set; }
    }
}
