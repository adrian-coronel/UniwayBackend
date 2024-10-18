using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Services.interfaces
{
    public interface IImagesServiceTechnicalService
    {
        Task<MessageResponse<ImagesServiceTechnical>> SaveAll(int ServiceTechnicalId, List<IFormFile> Files);
    }
}
