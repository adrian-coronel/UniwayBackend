using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("Experience")]
    public class Experience
    {
        public short Id { get; set; }
        public required string Name { get; set; }
    }
}
