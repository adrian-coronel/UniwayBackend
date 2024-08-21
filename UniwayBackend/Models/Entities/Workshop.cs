using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("Workshop")]
    public class Workshop
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Point? Location { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }
        public bool WorkingStatus { get; set; }

        public virtual TechnicalProfessionAvailability TechnicalProfessionAvailability { get; set; }

    }
}