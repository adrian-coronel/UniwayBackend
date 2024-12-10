using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalProfessionAvailabilityRequestRepository : BaseRepository<TechnicalProfessionAvailabilityRequest, int>, ITechnicalProfessionAvailabilityRequestRepository
    {
        public async Task<bool> DeleteRange(List<TechnicalProfessionAvailabilityRequest> techRequests)
        {
            using (DBContext context = new DBContext())
            {
                context.TechnicalProfessionAvailabilityRequests.RemoveRange(techRequests);
                return await context.SaveChangesAsync() > 0; // Guarda los cambios en la base de datos
            }
        }


        public async Task<List<TechnicalProfessionAvailabilityRequest>> FindAllPendingByRequestId(int RequestId, short stateRequestId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.TechnicalProfessionAvailabilityRequests
                    .Include(x => x.Request)
                    .Where(x => x.RequestId == RequestId && x.Request.StateRequestId == Constants.StateRequests.PENDING)
                    .ToListAsync();  
            }
        }

        public async Task<List<TechnicalProfessionAvailabilityRequestResponse>> FindAllPendingByUserId(Guid UserId)
        {
            using (DBContext context = new DBContext())
            {
                var result = new List<TechnicalProfessionAvailabilityRequestResponse>();

                // Cargar todas las disponibilidades
                var availabilities = await context.Availabilities.ToListAsync();

                // Recorrer todas las disponibilidades
                foreach (var availability in availabilities)
                {
                    var techProfRequest = new TechnicalProfessionAvailabilityRequestResponse
                    {
                        Availability = availability,
                        Requests = await context.Requests
                                    .Include(x => x.Client)
                                        .ThenInclude(y => y.User)
                                            .ThenInclude(d => d.PhotoUser)
                                    .Include(x => x.ImagesProblemRequests)
                                    .Include(x => x.ServiceTechnical)
                                        .ThenInclude(y => y.ServiceTechnicalTypeCars)
                                    .Include(x => x.StateRequest)
                                    .Where(x => x.TechnicalProfessionAvailabilityId != null &&
                                                x.TechnicalProfessionAvailability.AvailabilityId == availability.Id &&
                                                (x.StateRequestId == Constants.StateRequests.PENDING ||
                                                 x.StateRequestId == Constants.StateRequests.RESPONDING) &&
                                                x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId &&
                                                (availability.Id != 2 || x.TypeAttention == 2))
                                    .ToListAsync()
                    };

                    // Cargar solicitudes adicionales (solicitudes a muchos)
                    var additionalRequests = await context.TechnicalProfessionAvailabilityRequests
                        .Include(x => x.Request)
                            .ThenInclude(s => s.ServiceTechnical)
                        .Include(x => x.Request)
                            .ThenInclude(s => s.Client)
                                .ThenInclude(y => y.User)
                                    .ThenInclude(d => d.PhotoUser)
                        .Include(x => x.Request)
                            .ThenInclude(x => x.ImagesProblemRequests)
                        .Include(x => x.Request)
                            .ThenInclude(x => x.StateRequest)
                        .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId &&
                                    x.Request.StateRequestId == Constants.StateRequests.PENDING &&
                                    x.TechnicalProfessionAvailability.AvailabilityId == availability.Id &&
                                    (availability.Id != 2 || (x.Request.TypeAttention == 2 && x.Request.TechnicalProfessionAvailability.AvailabilityId == 2)))
                        .Select(x => x.Request)
                        .ToListAsync();

                    // Eliminar duplicados utilizando DistinctBy
                    techProfRequest.Requests = techProfRequest.Requests
                        .Concat(additionalRequests)
                        .DistinctBy(r => r.Id) // Eliminar duplicados según el Id
                        .ToList();

                    result.Add(techProfRequest);
                }

                return result;
            }
        }



    }
}
