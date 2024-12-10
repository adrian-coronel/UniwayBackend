using Azure.Core;
using Microsoft.EntityFrameworkCore;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalResponseRepository : BaseRepository<TechnicalResponse, int>, ITechnicalResponseRepository
    {
        public async Task<List<TechnicalResponse>> FindAllByClientIdAndRequest(int ClientId, int RequestId)
        {
            using (var context = new DBContext())
            {
                var query = context.TechnicalResponses
                    .Where(x => x.Request.ClientId == ClientId && (x.Request.StateRequestId==1 || x.Request.StateRequestId==6 ) && x.ProposedAssistanceDate !=null);

                // Aplica la condición adicional solo si RequestId es diferente de 0
                if (RequestId != 0)
                {
                    query = query.Where(x => x.Request.Id == RequestId);
                }

                // Incluye la entidad relacionada
                query = query.Include(x => x.TechnicalProfessionAvailability)
                                .ThenInclude(x=>x.TechnicalProfession)
                                    .ThenInclude(x=>x.UserTechnical)
                                        .ThenInclude(x=>x.Technical)
                              .Include(x=>x.TechnicalProfessionAvailability)
                                    .ThenInclude(x=>x.Workshops)
                              .Include(x=>x.Request)
                              .Include(x=>x.Request)
                                .ThenInclude(y=>y.ImagesProblemRequests)
                              ;

                return await query.ToListAsync();
            }
        }

        public async Task<TechnicalResponse> GetByRequestIdAndTechnicalProfessionAvailability(int RequestId,int TechnicalProfessionAvailabilityId)
        {
            using(var context=new DBContext())
            {
                return await context.TechnicalResponses.Where(x => x.RequestId == RequestId && x.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId).FirstOrDefaultAsync();
            }
        }
        public async Task<List<TechnicalResponse>> FindAllByRequestId(int RequestId)
        {
            using (var context = new DBContext())
            {
                return await context.TechnicalResponses
                    .Where(x => x.RequestId == RequestId)
                    .ToListAsync();
            }
        }
    }
}
