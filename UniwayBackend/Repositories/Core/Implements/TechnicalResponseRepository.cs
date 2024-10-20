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
                return await context.TechnicalResponses
                    .Where(x => x.Request.StateRequestId == Constants.StateRequests.PENDING &&
                                x.Request.ClientId == ClientId &&
                                x.Request.Id == RequestId)
                    .ToListAsync();
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
