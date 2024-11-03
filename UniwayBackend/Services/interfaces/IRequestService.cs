using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response.Request;

namespace UniwayBackend.Services.interfaces
{
    public interface IRequestService
    {
        Task<MessageResponse<Request>> GetAllByUser(Guid userId);
        Task<MessageResponse<Request>> GetAllScheduledRequest(int TechnicalProfessionAvailabilityId);
        Task<MessageResponse<RequestHistoryResponse>> GetAllHistoryByUser(Guid userId);
        Task<MessageResponse<Request>> GetRequestPendingInTrayByUserId(Guid userId);
        Task<MessageResponse<Request>> GetRequestPendingForClientAndStateRequest(int clientId, short? StateRequestId);
        Task<MessageResponse<Request>> Save(Request request, List<IFormFile> Files);
        Task<MessageResponse<Request>> UpdateStateRequestByRequestId(int requestId, short stateRequestId, int technicalProfessionAvailabilityId = 0);
    }
}
