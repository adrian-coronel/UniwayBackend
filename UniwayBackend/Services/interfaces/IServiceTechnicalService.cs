using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IServiceTechnicalService
    {
        Task<MessageResponse<ServiceTechnical>> GetByTechnicalAvailabilityId(int TechnicalAvailabilityId);
    }
}
