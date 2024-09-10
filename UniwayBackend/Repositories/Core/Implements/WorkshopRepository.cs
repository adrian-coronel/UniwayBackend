using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class WorkshopRepository : BaseRepository<Workshop, int>, IWorkshopRepository
    {

        /// <param name="referenceLocation">Punto de referencia de donde se tomará la distancia</param>
        /// <param name="distanceRadius">Distancia en metros</param>
        public async Task<List<Workshop>> FindDefaultLocation(Point referenceLocation, int distanceRadius)
        {
            using (var context = new DBContext())
            {
                return await context.Set<Workshop>()
                    .Where(x => x.TechnicalProfessionAvailability.AvailabilityId == Constants.Availabilities.IN_WORKSHOP_ID
                                && x.WorkingStatus
                                && !x.Location.IsEmpty && x.Location != null
                                && x.Location.Distance(referenceLocation) <= distanceRadius)
                    .ToListAsync();
            }
        }
    }
}
