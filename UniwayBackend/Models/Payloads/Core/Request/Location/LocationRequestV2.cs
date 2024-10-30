namespace UniwayBackend.Models.Payloads.Core.Request.Location
{
    public class LocationRequestV2
    {
        public int TechnicalProfessionAvailabilityId { get; set; }
        public int? WokrshopId { get; set; } // Si es que se quiere actualizar un workshop especifico
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}