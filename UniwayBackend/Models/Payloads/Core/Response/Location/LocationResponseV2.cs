using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;

namespace UniwayBackend.Models.Payloads.Core.Response.Location
{
    public class LocationResponseV2
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(PointConverter))]
        public Point? Location { get; set; }
        public bool WorkingStatus { get; set; }
        public short AvailabilityId { get; set; }
        public bool IsWorkshop { get; set; }

        public List<ServiceTechnicalResponse> Services { get; set; }
    }

}
