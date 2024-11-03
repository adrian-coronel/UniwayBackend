using Microsoft.EntityFrameworkCore;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class RequestRepository : BaseRepository<Request, int>, IRequestRepository
    {

        public async Task<List<Request>> FindAllByUser(Guid UserId)
        {
            using (DBContext context = new DBContext())
            {

                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == UserId);

                return await context.Requests
                    .Include(x => x.StateRequest)
                    .Include(x => x.ServiceTechnical)
                        .ThenInclude(x => x.Images)
                    .Where(x => 
                        ( user.RoleId == Constants.Roles.TECHNICAL_ID &&
                            x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId
                        )
                        ||
                        ( user.RoleId == Constants.Roles.CLIENT_ID &&
                            x.Client.UserId == UserId
                        )
                     )
                    .ToListAsync();
            }
        }

        public async Task<List<Request>> FindAllPendingByClientIdAndStateRequestId(int clientId, short stateRequestId = 0)
        {
            using (DBContext context = new DBContext())
            {
                var requests = await context.Requests
                    .Include(x => x.ImagesProblemRequests) // Incluir imágenes del problema
                    .Include(x => x.TechnicalResponses)    // Incluir respuestas técnicas
                        .ThenInclude(tr => tr.TechnicalProfessionAvailability) // Incluir disponibilidad técnica o de taller
                            .ThenInclude(tpa => tpa.TechnicalProfession) // Traer profesión del técnico
                                .ThenInclude(tp => tp.UserTechnical)     // Traer información del técnico
                                    .ThenInclude(tp => tp.Technical)
                    .Include(x => x.TechnicalResponses)    // Incluir respuestas técnicas
                        .ThenInclude(tr => tr.TechnicalProfessionAvailability) // Incluir disponibilidad técnica o de taller
                            .ThenInclude(tpa => tpa.Availability)
                    .Include(x => x.TechnicalResponses)    // Incluir respuestas técnicas
                        .ThenInclude(tr => tr.TechnicalProfessionAvailability) // Incluir disponibilidad técnica o de taller
                            .ThenInclude(tpa => tpa.Workshops)
                    .Where(x => x.ClientId == clientId 
                             && (stateRequestId == 0 || x.StateRequestId == stateRequestId)
                          )
                    .ToListAsync();

                return requests;
                //foreach (var request in requests)
                //{
                //    if (request.TechnicalProfessionAvailability.AvailabilityId == Constants.)
                //}
            }
        }


        public async Task<List<Request>> FindAllPendingByUserId(Guid UserId)
        {
            using (DBContext context = new DBContext())
            {
                var request = await context.TechnicalProfessionAvailabilityRequests
                    .Include(x => x.Request)
                        .ThenInclude(r => r.ImagesProblemRequests)
                    .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId
                             && !x.Request.TechnicalResponses.Any(tr =>
                                    tr.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId)
                             && x.Request.StateRequestId == Constants.StateRequests.PENDING)
                    .Select(x => x.Request)
                    .ToListAsync();

                var request2 = await context.Requests
                    .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId &&
                                x.StateRequestId == Constants.StateRequests.PENDING &&
                                !x.TechnicalResponses.Any(tr =>
                                    tr.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId)
                          )
                    .ToListAsync();

                // Combina ambas listas y elimina duplicados
                var combinedRequests = request
                    .Union(request2) // Combina ambas listas
                    .Distinct() // Elimina duplicados
                    .ToList();

                return combinedRequests;
            }
        }

        public async Task<List<Request>> FindAllScheduledRequest(int TechnicalProfessionAvailabilityId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Requests
                    .Include(x => x.StateRequest)
                    .Include(x => x.ServiceTechnical)
                        .ThenInclude(x => x.Images)
                    .Where(x => 
                                (x.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId || TechnicalProfessionAvailabilityId == 0) &&
                                x.StateRequestId == Constants.StateRequests.IN_PROCESS && // Solicitud aceptada
                                x.FromShow != null && x.ToShow != null && // Tienen un rango de fechas a mostrarse
                                x.TechnicalResponses.Any(x => x.ProposedAssistanceDate != null) // Si hay una fecha propuesta del mecánico
                     )
                    .ToListAsync();
            }
        }
    }
}
