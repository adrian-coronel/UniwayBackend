using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("UserTechnical")]
    public class UserTechnical
    {
        [Key]
        public int Id { get; set; }
        public int TechnicalId { get; set; }
        public Guid UserId { get; set; }
        public bool Enabled { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("TechnicalId")]
        public virtual Technical Technical {  get; set; }
        public virtual List<TechnicalProfession> TechnicalProfessions { get; set; }
        public virtual List<TowingCar> TowingCars { get; set; }

    }
}
