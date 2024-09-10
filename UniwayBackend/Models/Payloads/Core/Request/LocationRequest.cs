namespace UniwayBackend.Models.Payloads.Core.Request
{
    public class LocationRequest
    {
        public short AvailabilityId { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int Distance { get; set; }
    }
}
