using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalProfessionAvailabilityRequestService
    {
        Task<MessageResponse<TechnicalProfessionAvailabilityRequestResponse>> GetAllPendingByUserId(Guid UserId);
        
    }
}
