using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IPhotoUserRepository : IBaseRepository<PhotoUser, int>
    {
        Task<PhotoUser?> FindByUserId(Guid UserId);
        Task<PhotoUser?> FindByTechnicalId(int TechnicalId);
    }
}
