namespace UniwayBackend.Models.Payloads.Core.Request.ServiceTechnicalTypeCar;

public class ServiceTechnicalTypeCarRequest
{
    public int Id { get; set; }
    public int ServiceTechnicalId { get; set; }
    public short TypeCarId { get; set; }
    public decimal Price { get; set; }
}
