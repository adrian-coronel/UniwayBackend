using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("CategoryRequest", Schema = "dbo")]
    public class CategoryRequest
    {
        [Key]
        public short Id { get; set; }
        public string Name { get; set; }
    }
}
