using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
        Task<User?> FindByUsernameAndPassword(string Email, string Password);
        Task<User?> FindByIdAndRoleId(Guid Id, short RoleId);
        Task<User?> FindByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
    }
}
