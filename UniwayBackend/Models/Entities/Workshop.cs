using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UniwayBackend.Config;

namespace UniwayBackend.Models.Entities
{
    [Table("Workshop")]
    public class Workshop
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(PointConverter))]
        public Point? Location { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }
        public bool WorkingStatus { get; set; }

        [NotMapped]
        public double? Lat { get; set; }
        [NotMapped]
        public double? Lng { get; set; }

        [ForeignKey("TechnicalProfessionAvailabilityId")]
        public virtual TechnicalProfessionAvailability TechnicalProfessionAvailability { get; set; }

    }
}