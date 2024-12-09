using System.ComponentModel.DataAnnotations.Schema;

namespace UniwayBackend.Models.Entities
{
    [Table("User")]
    public class User
    {
        public Guid Id { get; set; }
        public short RoleId { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool Enabled { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public Role Role { get; set; }
        public virtual PhotoUser PhotoUser { get; set; }
        public virtual List<UserTechnical> UserTechnicals { get; set; }
    }
}
