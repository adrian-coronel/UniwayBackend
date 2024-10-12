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

                var availabilities = await context.Availabilities.ToListAsync();

                // Agremos las disponibilidades y solicitudes directas
                foreach (var availability in availabilities)
                {
                    var TechProfRequest = new TechnicalProfessionAvailabilityRequestResponse
                    {
                        Availability = availability,
                        Requests = await context.Requests
                                    .Where(x => x.TechnicalProfessionAvailabilityId != null &&
                                                x.TechnicalProfessionAvailability.AvailabilityId == availability.Id &&
                                                x.StateRequestId == Constants.StateRequests.PENDING &&
                                                x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId)
                                    .ToListAsync()
                    };

                    // Agregar solcitiudes hecha a muchos agregada en bandeja
                    TechProfRequest.Requests.AddRange(
                        await context.TechnicalProfessionAvailabilityRequests
                            .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId &&
                                        x.Request.StateRequestId == Constants.StateRequests.PENDING &&
                                        x.TechnicalProfessionAvailability.AvailabilityId == availability.Id)
                            .Select(x => x.Request)
                            .ToListAsync()
                    );

                    result.Add(TechProfRequest);
                }

                return result;
            }
        }


    }
}
