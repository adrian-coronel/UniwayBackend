using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IServiceTechnicalRepository : IBaseRepository<ServiceTechnical, int>
    {
        Task<List<ServiceTechnical>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
    }
}
