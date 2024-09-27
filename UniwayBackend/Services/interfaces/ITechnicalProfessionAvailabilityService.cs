using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalProfessionAvailabilityService
    {
        //Task<MessageResponse<TechnicalProfessionAvailability>> GetAllTechnicalLocations(int RangeDistance);
        Task<MessageResponse<TechnicalProfessionAvailability>> GetByTechnicalAndAvailability(int TechnicalId, short AvailabilityId);
        Task<MessageResponse<TechnicalProfessionAvailability>> GetByAvailabilityAndLocation(double lat, double lng, short AvailabilityId = 0, int distance = 5000);
    }
}
