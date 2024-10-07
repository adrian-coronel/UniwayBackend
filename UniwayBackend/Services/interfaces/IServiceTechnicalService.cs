using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IServiceTechnicalService
    {
        Task<List<ServiceTechnical>> GetByTechnicaIdAndAvailabilityId(int technicalId, short availabilityId);
        
    }
}
