using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.Technical;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalService
    {
        Task<MessageResponse<Technical>> GetInformation(int TechnicalId);
        Task<MessageResponse<Technical>> UpdateWorkinStatus(TechnicalRequestV1 request);
    }
}
