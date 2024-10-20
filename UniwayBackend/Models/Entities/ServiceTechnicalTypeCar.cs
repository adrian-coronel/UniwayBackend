using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("ServiceTechnicalTypeCar", Schema = "dbo")]
    public class ServiceTechnicalTypeCar
    {
        [Key]
        public int Id { get; set; }
        public short TypeCarId { get; set; }
        public int ServiceTechnicalId { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("TypeCarId")]
        public TypeCar TypeCar { get; set; }
    }
}
