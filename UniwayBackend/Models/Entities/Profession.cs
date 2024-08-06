using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("Profession")]
    public class Profession
    {
        public short Id { get; set; }
        public required string Name { get; set; }
    }
}
