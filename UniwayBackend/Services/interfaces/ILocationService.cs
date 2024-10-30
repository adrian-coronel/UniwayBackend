using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Location;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Location;

namespace UniwayBackend.Services.interfaces
{
    public interface ILocationService
    {
        Task<MessageResponse<LocationResponse>> GetAllByAvailability(LocationRequest request);
        Task<MessageResponse<LocationResponse>> UpdateByTechnicalProfessionAvailability(LocationRequestV2 request);
        Task<MessageResponse<LocationResponseV2>> GetAllByAvailabilityWithServices(LocationRequest request);
        Task<MessageResponse<LocationResponse>> GetByTechnicalProfessionAvailability(int TechnicalProfessionAvailabilityId);
    }
}
