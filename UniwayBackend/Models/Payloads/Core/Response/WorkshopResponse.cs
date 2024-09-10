using NetTopologySuite.Geometries;

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class WorkshopResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point? Location { get; set; }
        public int TechnicalOwnerId { get; set; }
        public bool WorkingStatus { get; set; }
    }
}
