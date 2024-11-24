using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalRepository : BaseRepository<Technical, int>, ITechnicalRepository
    {
        public async Task<Technical?> FindByTechnicalProfessionAvailability(int TechnicalProfessionAvailabilityId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.TechnicalProfessionAvailabilities
                    .Where(x => x.Id == TechnicalProfessionAvailabilityId)
                    .Select(x => x.TechnicalProfession.UserTechnical.Technical)
                    .FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Encuentra técnicos disponibles dentro de un radio específico y que cumplan ciertos criterios.
        /// </summary>
        /// <param name="referenceLocation">Punto de referencia para calcular la distancia.</param>
        /// <param name="distanceRadius">Radio de búsqueda en metros.</param>
        /// <returns>Lista de técnicos que cumplen los criterios.</returns>
        public async Task<List<Technical>> FindDefaultLocation(Point referenceLocation, int distanceRadius)
        {
            using (var context = new DBContext())
            {
                return await context.UserTechnicals.AsNoTracking()
                    .Where(x =>
                        x.User.RoleId == Constants.Roles.TECHNICAL_ID && // Filtra por rol de técnico
                        x.Technical.WorkingStatus && 
                        // Verificamos que la ubicación sea válida
                        x.Technical.Location != null &&
                        !x.Technical.Location.IsEmpty &&
                        x.Technical.Location.Distance(referenceLocation) <= distanceRadius && // Filtra por distancia al punto de referencia
                        // Verifica disponibilidad "AT_HOME"
                        context.TechnicalProfessionAvailabilities
                            .Any(tpa => tpa.AvailabilityId == Constants.Availabilities.AT_HOME_ID &&
                                        tpa.TechnicalProfession.UserTechnicalId == x.Id) &&
                        // Excluye técnicos con solicitudes en proceso
                        !(context.Requests
                            .Any(r => r.StateRequestId == Constants.StateRequests.IN_PROCESS &&
                                      r.TechnicalProfessionAvailability.TechnicalProfession.UserTechnicalId == x.Id))
                    )
                    .Include(x=>x.TechnicalProfessions)
                        .ThenInclude(x=>x.UserTechnical)
                    .Select(x => x.Technical)
                    .ToListAsync();
            }
        }

        public async Task<Technical> FindTechnicalWithInformation(int TechnicalId)
        {
            using (DBContext context = new DBContext())
            {
                var s =  await context.Technicals
                    .Include(x => x.Reviews) // Incluir Reviews
                    .Include(x => x.UserTechnicals)
                       .ThenInclude(x => x.TechnicalProfessions)
                            .ThenInclude(tp => tp.Profession) // Incluir Profession
                    .Include(x => x.UserTechnicals)
                        .ThenInclude(ut => ut.User)
                    .Include(x => x.UserTechnicals)
                        .ThenInclude(ut => ut.User)
                            .ThenInclude(u => u.Role)
                    .Include(x => x.UserTechnicals)
                        .ThenInclude(x => x.TechnicalProfessions)
                            .ThenInclude(tp => tp.Experience) // Incluir Experience
                    .Include(x => x.UserTechnicals)
                        .ThenInclude(ut => ut.TowingCars) // Incluir TowingCars
                    .Include(x => x.UserTechnicals)
                        .ThenInclude(x => x.TechnicalProfessions)
                            .ThenInclude(tp => tp.TechnicalProfessionAvailabilities)
                                .ThenInclude(tpa => tpa.Availability)
                    .Include(x => x.UserTechnicals)
                        .ThenInclude(x => x.TechnicalProfessions)
                            .ThenInclude(tp => tp.TechnicalProfessionAvailabilities)
                                .ThenInclude(tpa => tpa.Workshops) // Incluir Workshops
                    .FirstAsync(x => x.Id == TechnicalId);
                return s;
            }
        }

        public async Task<Technical> FindTechnicalWithInformationByUser(Guid userId)
        {
            using (DBContext context = new DBContext())
            {
                var technical = await context.Technicals
                    .Include(t => t.Reviews) // Incluir Reviews
                    .Include(t => t.UserTechnicals)
                        .ThenInclude(ut => ut.TechnicalProfessions)
                            .ThenInclude(tp => tp.Profession) // Incluir Profession
                    .Include(t => t.UserTechnicals)
                        .ThenInclude(ut => ut.TechnicalProfessions)
                            .ThenInclude(tp => tp.Experience) // Incluir Experience
                    .Include(t => t.UserTechnicals)
                        .ThenInclude(ut => ut.TechnicalProfessions)
                            .ThenInclude(tp => tp.TechnicalProfessionAvailabilities)
                                .ThenInclude(tpa => tpa.Availability) // Incluir Availability
                    .Include(t => t.UserTechnicals)
                        .ThenInclude(ut => ut.TechnicalProfessions)
                            .ThenInclude(tp => tp.TechnicalProfessionAvailabilities)
                                .ThenInclude(tpa => tpa.Workshops) // Incluir Workshops
                    .Include(t => t.UserTechnicals)
                        .ThenInclude(ut => ut.User)
                            .ThenInclude(u => u.Role) // Incluir Role
                    .FirstOrDefaultAsync(t => t.UserTechnicals.Any(ut => ut.UserId == userId));

                return technical;
            }
        }


        public async Task<List<UserRequest>> UpdateWorkingStatus(int TechnicalId, bool WorkingStatus, double Lat, double Lng, int Distance = 5000)
        {
            using (DBContext context = new DBContext())
            {
                var technicalIdParameter = new SqlParameter("@TechnicalId", TechnicalId);
                var workingStatusParameter = new SqlParameter("@WorkingStatus", WorkingStatus);
                var distanceParameter = new SqlParameter("@Distance", Distance);
                var LatitudeParameter = new SqlParameter("@Lat", Lat);
                var LongitudeParameter = new SqlParameter("@Lng", Lng);

                List<UserRequest> result = await context.UserRequests
                    .FromSqlRaw("EXEC sp_updateWorkingStatusForTechnical @TechnicalId, @WorkingStatus, @Distance, @Lat, @Lng",
                      technicalIdParameter, workingStatusParameter, distanceParameter, LatitudeParameter, LongitudeParameter)
                    .ToListAsync();

                return result;
            }
        }
    }
}
