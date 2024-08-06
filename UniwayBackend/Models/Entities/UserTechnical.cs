using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("UserTechnical")]
    public class UserTechnical
    {

        public int Id { get; set; }
        public int TechnicalId { get; set; }
        public Guid UserId { get; set; }
        public bool Enabled { get; set; }


        public virtual User User { get; set; }
        public virtual Technical Technical {  get; set; }

    }
}
