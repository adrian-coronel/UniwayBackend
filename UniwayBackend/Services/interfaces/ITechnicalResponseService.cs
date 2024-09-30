using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalResponseService
    {
        Task<MessageResponse<TechnicalResponse>> Save(TechnicalResponse technicalResponse);
    }
}
