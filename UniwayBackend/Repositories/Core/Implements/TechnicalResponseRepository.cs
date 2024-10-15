using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalResponseRepository : BaseRepository<TechnicalResponse, int>, ITechnicalResponseRepository
    {
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
