using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TechnicalProfessionAvailabilityRequest")]
    public class TechnicalProfessionAvailabilityRequest
    {
        [Key]
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }

        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        [ForeignKey("TechnicalProfessionAvailabilityId")]
        public virtual TechnicalProfessionAvailability TechnicalProfessionAvailability { get; set; }
    }
}
