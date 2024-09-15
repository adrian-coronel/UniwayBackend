using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using UniwayBackend.Config;

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class WorkshopResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(PointConverter))]
        public Point? Location { get; set; }
        public int TechnicalOwnerId { get; set; }
        public bool WorkingStatus { get; set; }
    }
}
