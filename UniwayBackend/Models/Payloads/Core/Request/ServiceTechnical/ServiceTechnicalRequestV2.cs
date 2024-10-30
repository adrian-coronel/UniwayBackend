namespace UniwayBackend.Models.Payloads.Core.Request.ServiceTechnical
{
    public class ServiceTechnicalRequestV2
    {
        public int Id { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }
        public short CategoryServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
