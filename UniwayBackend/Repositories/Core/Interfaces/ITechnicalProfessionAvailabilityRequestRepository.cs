using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalProfessionAvailabilityRequestRepository : IBaseRepository<TechnicalProfessionAvailabilityRequest, int>
    {
        Task<List<TechnicalProfessionAvailabilityRequest>> FindAllPendingByRequestId(int RequestId, short stateRequestId);
        Task<bool> DeleteRange(List<TechnicalProfessionAvailabilityRequest> techRequests);
        Task<List<TechnicalProfessionAvailabilityRequestResponse>> FindAllPendingByUserId(Guid UserId);
    }
}
