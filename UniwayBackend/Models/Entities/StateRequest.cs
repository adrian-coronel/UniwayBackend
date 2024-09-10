using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("StateRequest")]
    public class StateRequest
    {
        [Key]
        public short Id { get; set; }
        public string Name { get; set; }
    }
}
