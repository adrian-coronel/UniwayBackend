﻿using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UniwayBackend.Models.Entities
{
    [Table("Request", Schema = "dbo")]
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public int? TypeAttention { get; set; }
        public short StateRequestId { get; set; }
        public short CategoryRequestId { get; set; }
        public short? TypeCarId { get; set; }
        public int ClientId { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public int? ServiceTechnicalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Point Location { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        [NotMapped]
        [JsonIgnore]
        public DateTime? ProposeAssistanceByTechnicalAttended { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public DateTime? FromShow { get; set; }
        public DateTime? ToShow { get; set; }
        public bool IsResponse { get; set; }

        [ForeignKey("TechnicalProfessionAvailabilityId")]
        public virtual TechnicalProfessionAvailability TechnicalProfessionAvailability { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        [ForeignKey("StateRequestId")]
        public virtual StateRequest StateRequest { get; set; }
        [ForeignKey("CategoryRequestId")]
        public virtual CategoryRequest CategoryRequest { get; set; }
        [ForeignKey("ServiceTechnicalId")]
        public virtual ServiceTechnical ServiceTechnical { get; set; }
        [ForeignKey("TypeCarId")]
        public virtual TypeCar TypeCar { get; set; }


        public virtual List<Review> Reviews { get; set; }
        public virtual List<TechnicalResponse> TechnicalResponses { get; set; }
        public virtual List<ImagesProblemRequest> ImagesProblemRequests { get; set; }
    }
}
