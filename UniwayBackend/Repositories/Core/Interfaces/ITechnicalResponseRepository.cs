using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalResponseRepository : IBaseRepository<TechnicalResponse, int>
    {
        Task<List<TechnicalResponse>> FindAllByRequestId(int RequestId);   
    }
}
