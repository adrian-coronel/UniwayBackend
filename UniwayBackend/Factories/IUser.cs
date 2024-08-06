using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Auth;

namespace UniwayBackend.Factories
{
    public interface IUser
    {
        int GetRoleId();
        Task<User> Create(RegisterRequest request);
    }
}
