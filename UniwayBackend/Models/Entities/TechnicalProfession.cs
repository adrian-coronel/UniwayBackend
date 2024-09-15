using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TechnicalProfession")]
    public class TechnicalProfession
    {
        public int Id { get; set; }
        public short ExperienceId { get; set; }
        public short ProfessionId { get; set; }
        public int UserTechnicalId { get; set; }

        [ForeignKey("ProfessionId")]
        public virtual Profession Profession { get; set; }
        [ForeignKey("ExperienceId")]
        public virtual Experience Experience { get; set; }
        [ForeignKey("UserTechnicalId")]
        public virtual UserTechnical UserTechnical { get; set; }
        public virtual List<TechnicalProfessionAvailability> TechnicalProfessionAvailabilities { get; set; }
    }
}
