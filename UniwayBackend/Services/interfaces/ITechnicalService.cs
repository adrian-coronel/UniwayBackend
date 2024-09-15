using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ITechnicalService
    {
        Task<MessageResponse<Technical>> GetInformation(int TechnicalId);
    }
}
