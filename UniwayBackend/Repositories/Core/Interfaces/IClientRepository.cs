using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client, int>
    {
        Task<Client?> FindByUserId(Guid UserId);
    }
}
