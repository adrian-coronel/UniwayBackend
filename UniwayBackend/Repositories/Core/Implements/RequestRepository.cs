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
                return await context.TechnicalProfessionAvailabilityRequests
                    .Include(x => x.Request)
                        .ThenInclude(r => r.ImagesProblemRequests)
                    .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId
                             && !x.Request.TechnicalResponses.Any(tr =>
                                    tr.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId)
                             && x.Request.StateRequestId == Constants.StateRequests.PENDING)
                    .Select(x => x.Request)
                    .ToListAsync();
            }
        }

    }
}
