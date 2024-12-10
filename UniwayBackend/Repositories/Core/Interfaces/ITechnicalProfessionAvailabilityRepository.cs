using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalProfessionAvailabilityRepository : IBaseRepository<TechnicalProfessionAvailability, int>
    {
        //Task<List<TechnicalProfessionAvailability>> FindAllByWorkshopStatusAndAvailability(bool WorkingStatus, short AvailabilityId);
        Task<TechnicalProfessionAvailability> FindByTechnicalAndAvailability(int technicalId, short availabilityId);
        Task<List<TechnicalProfessionAvailability>> FindByAvailabilityAndLocation(Point point, short availabilityId = 0, int distance = 5000);

        Task<TechnicalProfessionAvailability> FindTechnicalInformationByTechnicalProfeessionAvailabilityId(int TechnicalProfessionAvailabilityId);
    }
}
