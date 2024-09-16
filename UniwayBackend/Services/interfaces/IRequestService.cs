using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IRequestService
    {
        Task<MessageResponse<Request>> Save(Request request);
    }
}
