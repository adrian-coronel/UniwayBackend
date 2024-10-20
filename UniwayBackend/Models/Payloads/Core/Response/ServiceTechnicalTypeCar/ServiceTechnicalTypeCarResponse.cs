using UniwayBackend.Models.Payloads.Core.Response.TypeCar;

namespace UniwayBackend.Models.Payloads.Core.Response.ServiceTechnicalTypeCar
{
    public class ServiceTechnicalTypeCarResponse
    {
        public int Id { get; set; }
        public short TypeCarId { get; set; }
        public int ServiceTechnicalId { get; set; }
        public decimal Price { get; set; }

        public TypeCarResponse TypeCar { get; set; }
    }
}
