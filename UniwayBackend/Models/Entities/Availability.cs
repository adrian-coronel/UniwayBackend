using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("Availability")]
    public class Availability
    {
        public short Id { get; set; }
        public required string Name { get; set; }
    }
}
