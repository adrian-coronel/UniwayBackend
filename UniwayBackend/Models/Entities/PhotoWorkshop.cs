using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Entities
{
    [Table(nameof(PhotoWorkshop), Schema = "dbo")]
    public class PhotoWorkshop
    {
        [Key]
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey(nameof(WorkshopId))]
        public virtual Workshop Workshop { get; set; }
    }
}
