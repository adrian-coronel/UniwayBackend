using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IPhotoWorkshopRepository : IBaseRepository<PhotoWorkshop, int>
    {
        Task<PhotoWorkshop?> FindByWorkshopId(int workshopId);
    }
}
