using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("ServiceTechnical", Schema = "dbo")]
    public class ServiceTechnical
    {
        public int Id { get; set; }
        public int TechnicalProfessionAvailabilityId { get; set; }
        public short CategoryServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("TechnicalProfessionAvailabilityId")]
        public virtual TechnicalProfessionAvailability TechnicalProfessionAvailability { get; set; }
        [ForeignKey("CategoryServiceId")]
        public virtual CategoryService CategoryService { get; set; }
    }
}
