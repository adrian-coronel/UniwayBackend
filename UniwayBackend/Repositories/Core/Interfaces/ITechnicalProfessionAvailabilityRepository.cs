using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalProfessionAvailabilityRepository : IBaseRepository<TechnicalProfessionAvailability, int>
    {
        //Task<List<TechnicalProfessionAvailability>> FindAllByWorkshopStatusAndAvailability(bool WorkingStatus, short AvailabilityId);
        Task<TechnicalProfessionAvailability> FindByTechnicalAndAvailability(int technicalId, short availabilityId);
        Task<List<TechnicalProfessionAvailability>> FindByAvailabilityAndLocation(short availabilityId, Point point, int distance);
    }
}
