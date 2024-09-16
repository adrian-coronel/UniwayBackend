using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IStateRequestService
    {
        Task<MessageResponse<StateRequest>> GetAll();
    }
}
