using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Entities
{
    [Table("Review",  Schema = "dbo")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int TechnicalId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Range(1, 5)]
        public short StarNumber { get; set; }

        [StringLength(80)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        // Navigation properties
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        [ForeignKey("TechnicalId")]
        public virtual Technical Technical { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
