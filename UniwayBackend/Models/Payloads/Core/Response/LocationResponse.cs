namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class LocationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool WorkingStatus { get; set; }
        public bool IsWorkshop { get; set; }
    }
}
