using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("ImagesServiceTechnical", Schema = "dbo")]
    public class ImagesServiceTechnical
    {
        [Key]
        public int Id { get; set; }
        public int ServiceTechnicalId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }        
    }
}
