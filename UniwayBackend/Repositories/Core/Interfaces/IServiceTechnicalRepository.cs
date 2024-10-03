using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IServiceTechnicalRepository : IBaseRepository<ServiceTechnical, int>
    {
        Task<List<ServiceTechnical>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId);
        Task<List<ServiceTechnical>> FindFiveByTechnicalId(int TechnicalId);
        Task<List<ServiceTechnical>> FindFiveByWorkshopId(int WorkshopId);
    }
}
