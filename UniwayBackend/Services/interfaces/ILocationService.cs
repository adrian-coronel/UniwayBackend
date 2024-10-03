using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Location;

namespace UniwayBackend.Services.interfaces
{
    public interface ILocationService
    {
        Task<MessageResponse<LocationResponse>> GetAllByAvailability(LocationRequest request);
        Task<MessageResponse<LocationResponseV2>> GetAllByAvailabilityWithServices(LocationRequest request);
    }
}
