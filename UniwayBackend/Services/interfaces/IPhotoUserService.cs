using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.PhotoUser;

namespace UniwayBackend.Services.interfaces
{
    public interface IPhotoUserService
    {
        Task<MessageResponse<PhotoUser>> Save(PhotoWorkshopRequest photo);
    }
}
