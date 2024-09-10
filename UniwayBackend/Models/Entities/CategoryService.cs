using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("CategoryService", Schema = "dbo")]
    public class CategoryService
    {
        [Key]
        public short Id { get; set; }
        public string Name { get; set; }
    }
}