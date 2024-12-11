using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalProfessionRepository : IBaseRepository<TechnicalProfession, int>
    {
        Task<TechnicalProfession?> FindByTechnicalId(int technicalId);
    }
}
