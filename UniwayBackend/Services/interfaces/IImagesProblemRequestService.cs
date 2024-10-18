using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IImagesProblemRequestService
    {
        Task<MessageResponse<ImagesProblemRequest>> Save(int RequestId, IFormFile file);
        Task<MessageResponse<ImagesProblemRequest>> SaveAll(int RequestId, List<IFormFile> files);
    }
}
