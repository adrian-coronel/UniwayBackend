using UniwayBackend.Models.Entities;

namespace UniwayBackend.Models.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual RoleDto Role { get; set; }
    }
}
