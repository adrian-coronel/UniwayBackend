using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ITechnicalRepository : IBaseRepository<Technical, int>
    {
        /// <summary>
        /// Encuentra técnicos disponibles dentro de un radio específico y que cumplan ciertos criterios.
        /// </summary>
        /// <param name="referenceLocation">Punto de referencia para calcular la distancia.</param>
        /// <param name="distanceRadius">Radio de búsqueda en metros.</param>
        /// <returns>Lista de técnicos que cumplen los criterios.</returns>
        Task<List<Technical>> FindDefaultLocation(Point referenceLocation, int distanceRadius);
        Task<Technical> FindTechnicalWithInformation(int TechnicalId);
        Task<Technical> FindTechnicalWithInformationByUser(Guid userId);
        Task<Technical?> FindByTechnicalProfessionAvailability(int TechnicalProfessionAvailabilityId);
        Task<List<UserRequest>> UpdateWorkingStatus(int TechnicalId, bool WorkingStatus, double Lat, double Lng, int Distance = 5000);
    }
}
