using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalResponseService
    {
        Task<MessageResponse<TechnicalResponse>> GetAllByRequestId(int RequestId);
        Task<MessageResponse<TechnicalResponse>> GetAllByClientIdAndRequestId(int ClientId, int RequestId);
        Task<MessageResponse<TechnicalResponse>> Save(TechnicalResponse technicalResponse);
    }
}
