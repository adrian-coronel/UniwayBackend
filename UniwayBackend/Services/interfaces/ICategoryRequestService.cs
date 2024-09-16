using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface ICategoryRequestService
    {
        Task<MessageResponse<CategoryRequest>> GetAll();
    }
}
