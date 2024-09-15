namespace UniwayBackend.Models.Payloads.Core.Response.TowingCar
{
    public class TowingCarResponse
    {
        public int Id { get; set; }
        public int UserTechnicalId { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public short Year { get; set; }
    }
}
