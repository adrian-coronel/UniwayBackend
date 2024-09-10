using NetTopologySuite.Geometries;

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class TechnicalResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherLastname { get; set; }
        public string MotherLastname { get; set; }
        public string Dni { get; set; }
        public DateTime BirthDate { get; set; }
        public Point? Location { get; set; }
        public bool WorkingStatus { get; set; }
    }
}
