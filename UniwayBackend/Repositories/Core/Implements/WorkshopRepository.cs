﻿using Microsoft.Data.SqlClient;
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
        public async Task<List<Workshop>> FindAllByTechnicalProfessionAvailability(int TechProfAvaId, int? WorkshopId)
        {
            using (var context = new DBContext())
            {
                return await context.Workshops
                    .Include(w => w.PhotoWorkshop)
                    .Where(x => x.TechnicalProfessionAvailabilityId == TechProfAvaId &&
                               (WorkshopId == null || x.Id == WorkshopId)
                    ).ToListAsync();
            }
        }

        /// <param name="referenceLocation">Punto de referencia de donde se tomará la distancia</param>
        /// <param name="distanceRadius">Distancia en metros</param>
        public async Task<List<Workshop>> FindDefaultLocation(Point referenceLocation, int distanceRadius)
        {
            using (var context = new DBContext())
            {
                return await context.Set<Workshop>()
                    .Include(w => w.PhotoWorkshop)
                    .Where(x => x.TechnicalProfessionAvailability.AvailabilityId == Constants.Availabilities.IN_WORKSHOP_ID
                                && x.WorkingStatus
                                && !x.Location.IsEmpty && x.Location != null
                                && x.Location.Distance(referenceLocation) <= distanceRadius)
                    .Include(x=>x.TechnicalProfessionAvailability)
                        .ThenInclude(x=>x.TechnicalProfession)
                            .ThenInclude(x=>x.UserTechnical)
                    .ToListAsync();
            }
        }

        public async Task<List<Workshop>> UpdateAll(List<Workshop> workshops)
        {
            using (var context = new DBContext())
            {
                foreach (var workshop in workshops)
                {
                    // Marcamos cada Workshop como modificado en el contexto
                    context.Entry(workshop).State = EntityState.Modified;
                }

                // Guardamos todos los cambios en la base de datos
                await context.SaveChangesAsync();

                return workshops; // Devolvemos la lista actualizada
            }
        }


        public async Task<List<UserRequest>> UpdateWorkingStatus(int WorkshopId, bool WorkingStatus, double Lat, double Lng, int Distance = 5000)
        {
            using (DBContext context = new DBContext())
            {
                var workshopIdParameter = new SqlParameter("@WorkshopId", WorkshopId);
                var workingStatusParameter = new SqlParameter("@WorkingStatus", WorkingStatus);
                var distanceParameter = new SqlParameter("@Distance", Distance);
                var LatitudeParameter = new SqlParameter("@Lat", Lat);
                var LongitudeParameter = new SqlParameter("@Lng", Lng);

                List<UserRequest> result = await context.UserRequests
                    .FromSqlRaw("EXEC sp_updateWorkingStatusForWorkshop @WorkshopId, @WorkingStatus, @Distance, @Lat, @Lng",
                      workshopIdParameter, workingStatusParameter, distanceParameter, LatitudeParameter, LongitudeParameter)
                    .ToListAsync();

                return result;
            }
        }
    }
}
