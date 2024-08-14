using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("WorkshopTechnicalProfession")]
    public class WorkshopTechnicalProfession
    {
        [Key]
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public int TechnicalProfessionId { get; set; }
        public bool Enabled { get; set; }

        public virtual Workshop Workshop { get; set; }
        public virtual TechnicalProfession TechnicalProfession { get; set; }
    }
}
