using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ICategoryServiceRepository : IBaseRepository<CategoryService, short>
    {
        Task<List<CategoryService>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<CategoryService?> FindByIdAndTechnicalProfessionAvailabilityId(short CategoryServiceId, int TechnicalProfessionAvailabilityId);
    }
}
