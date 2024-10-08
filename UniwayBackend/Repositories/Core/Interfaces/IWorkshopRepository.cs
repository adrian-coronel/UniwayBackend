using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IWorkshopRepository : IBaseRepository<Workshop, int>
    {
        /// <param name="referenceLocation">Punto de referencia de donde se tomará la distancia</param>
        /// <param name="distanceRadius">Distancia en metros</param>
        Task<List<Workshop>> FindDefaultLocation(Point referenceLocation, int distanceRadius);
        Task<List<UserRequest>> UpdateWorkingStatus(int WorkshopId, bool WorkingStatus, double Lat, double Lng, int Distance = 5000);
    }
}
