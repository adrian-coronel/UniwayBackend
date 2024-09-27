using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalProfessionAvailabilityService
    {
        //Task<MessageResponse<TechnicalProfessionAvailability>> GetAllTechnicalLocations(int RangeDistance);
        Task<MessageResponse<TechnicalProfessionAvailability>> GetByTechnicalAndAvailability(int TechnicalId, short AvailabilityId);
        Task<MessageResponse<TechnicalProfessionAvailability>> GetByAvailabilityAndLocation(Point point, short AvailabilityId = 0, int distance = 5000);
    }
}
