using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("Role")]
    public class Role
    {
        public short Id { get; set; }
        public required string Name { get; set; }
    }
}
