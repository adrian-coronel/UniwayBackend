﻿using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response.Client;
using UniwayBackend.Models.Payloads.Core.Response.ImageProblem;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;
using UniwayBackend.Models.Payloads.Core.Response.StateRequest;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;

namespace UniwayBackend.Models.Payloads.Core.Response.Request
{
    public class RequestResponse
    {
        public int Id { get; set; }
        public int TypeAttention { get; set; }
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public short TypeCarId { get; set; }
        public int ClientId { get; set; }
        public ClientResponseV2 Client { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public TechnicalProfessionAvailabilityResponse TechnicalProfessionAvailability { get; set; }
        public ServiceTechnicalResponse ServiceTechnical { get; set; }
        public  StateRequestResponse StateRequest { get; set; }

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

        public DateTime? ProposeAssistanceByTechnicalAttended { get; set; }

        public List<ImagesProblemRequestResponse> ImagesProblemRequests { get; set; }
    }
}
