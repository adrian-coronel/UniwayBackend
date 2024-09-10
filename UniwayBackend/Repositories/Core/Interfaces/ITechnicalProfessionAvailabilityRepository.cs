using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalProfessionAvailabilityRepository : IBaseRepository<TechnicalProfessionAvailability, int>
    {
        //Task<List<TechnicalProfessionAvailability>> FindAllByWorkshopStatusAndAvailability(bool WorkingStatus, short AvailabilityId);
    }
}
