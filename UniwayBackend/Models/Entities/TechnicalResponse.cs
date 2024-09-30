using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TechnicalResponse")]
    public class TechnicalResponse
    {
        [Key]
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int? TechnicalProfessionAvailabilityId { get; set; }
        public int? WorkshopTechnicalProfessionId { get; set; }
        public string Title { get; set; }
        public string Description {  get; set; }
        public double Price { get; set; }
        public DateTime? ProposedAssistanceDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        [ForeignKey("TechnicalProfessionAvailabilityId")]
        public virtual TechnicalProfessionAvailability? TechnicalProfessionAvailability { get; set; }
        [ForeignKey("WorkshopTechnicalProfessionId")]
        public virtual WorkshopTechnicalProfession? WorkshopTechnicalProfession { get; set; }

        // Relations
        public virtual List<Material> Materials { get; set; }
    }
}
