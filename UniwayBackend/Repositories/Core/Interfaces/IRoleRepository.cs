using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role, short>
    {
        // Metodos personalizados
        Task<Role> CreateAsync(Role role);
    }
}
