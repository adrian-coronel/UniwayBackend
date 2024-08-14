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

        public virtual Availability Availability { get; set; }
        public virtual TechnicalProfession TechnicalProfession { get; set; }
    }
}