using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IRequestService
    {
        Task<MessageResponse<Request>> GetRequestPendingInTrayByUserId(Guid userId);
        Task<MessageResponse<Request>> GetRequestPendingForClientAndStateRequest(int clientId, short? StateRequestId);
        Task<MessageResponse<Request>> Save(Request request);
        Task<MessageResponse<Request>> UpdateStateRequestByRequestId(int requestId, short stateRequestId, int technicalProfessionAvailabilityId = 0);
    }
}
