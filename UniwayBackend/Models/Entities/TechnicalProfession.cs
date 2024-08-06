using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("TechnicalProfession")]
    public class TechnicalProfession
    {
        public int Id { get; set; }
        public short ExperienceId { get; set; }
        public short ProfessionId {  get; set; }
        public int UserTechnicalId { get; set; }

        public virtual UserTechnical UserTechnical { get; set; }
    }
}
