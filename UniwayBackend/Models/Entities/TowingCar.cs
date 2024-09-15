using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TowingCar")]
    public class TowingCar
    {
        [Key]
        public int Id { get; set; }
        public int UserTechnicalId { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public short Year { get; set; }

        [ForeignKey("UserTechnicalId")]
        public virtual UserTechnical UserTechnical { get; set; }
    }
}
