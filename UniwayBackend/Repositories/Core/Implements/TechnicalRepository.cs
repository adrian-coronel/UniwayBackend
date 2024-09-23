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
                    .Select(x => x.Technical)
                    .ToListAsync();
            }
        }

        public async Task<Technical> FindTechnicalWithInformation(int TechnicalId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Technicals
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
            }
        }
    }
}
