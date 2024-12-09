using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Entities
{
    [Table("PhotoUser", Schema = "dbo")]
    public class PhotoUser
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
