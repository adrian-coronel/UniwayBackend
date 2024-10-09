using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TechnicalProfessionAvailability")]
    public class TechnicalProfessionAvailability
    {
        [Key]
        public int Id { get; set; }
        public short AvailabilityId { get; set; }
        public int TechnicalProfessionId { get; set; }

        [ForeignKey("AvailabilityId")]
        public virtual Availability Availability { get; set; }
        [ForeignKey("TechnicalProfessionId")]
        public virtual TechnicalProfession TechnicalProfession { get; set; }

        public virtual List<Workshop> Workshops { get; set; } = new List<Workshop>();
    }
}