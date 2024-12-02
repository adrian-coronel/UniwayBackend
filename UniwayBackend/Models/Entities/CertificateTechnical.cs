using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Entities
{
    [Table("CertificateTechnical", Schema = "dbo")]
    public class CertificateTechnical
    {
        [Key]
        public int Id { get; set; }
        public int TechnicalId { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("TechnicalId")]
        public virtual Technical Technical { get; set; }
    }
}
