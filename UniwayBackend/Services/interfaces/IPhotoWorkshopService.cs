using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;

namespace UniwayBackend.Services.interfaces
{
    public interface IPhotoWorkshopService
    {
        Task<MessageResponse<PhotoWorkshop>> Save(PhotoWorkshopRequest photo);
    }
}
