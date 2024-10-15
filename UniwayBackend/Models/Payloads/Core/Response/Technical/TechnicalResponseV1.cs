using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using UniwayBackend.Config;

namespace UniwayBackend.Models.Payloads.Core.Response.Technical
{
    public class TechnicalResponseV1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherLastname { get; set; }
        public string MotherLastname { get; set; }
        public string Dni { get; set; }
        public DateTime BirthDate { get; set; }
        [JsonConverter(typeof(PointConverter))]
        public Point? Location { get; set; }
        public bool WorkingStatus { get; set; }
    }
}
