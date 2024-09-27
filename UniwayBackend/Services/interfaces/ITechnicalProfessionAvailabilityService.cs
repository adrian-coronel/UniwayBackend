using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalProfessionAvailabilityService
    {
        //Task<MessageResponse<TechnicalProfessionAvailability>> GetAllTechnicalLocations(int RangeDistance);
        Task<MessageResponse<TechnicalProfessionAvailability>> GetByTechnicalAndAvailability(int TechnicalId, short AvailabilityId);
    }
}
