using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ILocationService
    {
        Task<MessageResponse<LocationResponse>> GetAllByAvailability(LocationRequest request);
    }
}
