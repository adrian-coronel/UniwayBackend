using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual RoleResponse Role { get; set; }
    }
}
