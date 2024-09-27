using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using UniwayBackend.Config;

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class LocationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(PointConverter))]
        public Point? Location { get; set; }
        public bool WorkingStatus { get; set; }
        public short AvailabilityId { get; set; }
        public bool IsWorkshop { get; set; }
    }
}
