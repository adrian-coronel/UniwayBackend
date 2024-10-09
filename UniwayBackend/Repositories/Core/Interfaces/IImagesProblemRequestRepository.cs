using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IImagesProblemRequestRepository : IBaseRepository<ImagesProblemRequest, int>
    {
        Task<List<ImagesProblemRequest>> FindAllByRequestId(int RequestId);
    }
}
