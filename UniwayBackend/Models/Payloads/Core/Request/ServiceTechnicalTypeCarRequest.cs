namespace UniwayBackend.Models.Payloads.Core.Request
{
    public class ServiceTechnicalTypeCarRequest
    {
        public int ServiceTechnicalId { get; set; }
        public short TypeCarId { get; set; }
        public decimal Price { get; set; }
    }
}
