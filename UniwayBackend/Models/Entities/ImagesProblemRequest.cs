using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("ImagesProblemRequest")]
    public class ImagesProblemRequest
    {
        [Key]
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string ExtensionType { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        
    }
}
