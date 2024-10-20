using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TypeCar", Schema = "dbo")]
    public class TypeCar
    {
        [Key]
        public short Id { get; set; }
        public string Name { get; set; }
    }
}
