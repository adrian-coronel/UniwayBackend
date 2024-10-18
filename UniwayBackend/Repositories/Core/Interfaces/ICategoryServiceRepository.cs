using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ICategoryServiceRepository : IBaseRepository<CategoryService, short>
    {
        Task<List<CategoryService>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<List<CategoryService>> FindByIdAndAvailabilityId(short? CategoryServiceId = null, int? TechnicalProfessionAvailabilityId = null,short ? AvailablityId = null);
    }
}
