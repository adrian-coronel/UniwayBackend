using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IImagesProblemRequestService
    {
        Task<MessageResponse<ImagesProblemRequest>> Save(ImagesProblemRequest imagesProblemRequest);
        Task<MessageResponse<ImagesProblemRequest>> SaveAll(List<ImagesProblemRequest> imagesProblemRequests);
    }
}
