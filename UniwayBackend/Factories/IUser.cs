using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Request;

namespace UniwayBackend.Factories
{
    public interface IUser
    {
        int GetRoleId();
        Task<User> Create(RegisterRequest request);
        Task<User> Edit(ProfileRequest request);
        Task<User> Delete(Guid UserId);
    }
}
