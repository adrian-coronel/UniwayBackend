using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;


namespace UniwayBackend.Repositories.Core.Implements
{
    public class RoleRepository : BaseRepository<Role, short>, IRoleRepository
    {
        public Task<Role> CreateAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
