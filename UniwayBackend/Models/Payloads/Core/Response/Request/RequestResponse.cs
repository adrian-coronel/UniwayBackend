using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response.ImageProblem;

namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestResponse
    {
        public int Id { get; set; }
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public short TypeCarId { get; set; }
        public int ClientId { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public int? ServiceTechnicalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(PointConverter))]
        public Point? Location { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public bool IsResponse { get; set; }
        public DateTime? CreatedOn { get; set; }

        public List<ImagesProblemRequestResponse> ImagesProblemRequests { get; set; }
    }
}
