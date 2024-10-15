using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IRequestRepository : IBaseRepository<Request, int>
    {
        Task<List<Request>> FindAllPendingByUserId(Guid UserId);
        Task<List<Request>> FindAllPendingByClientIdAndStateRequestId(int clientId, short stateRequestId = 0);
    }
}
