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
        public async Task<List<Request>> FindAllPendingByUserId(Guid UserId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.TechnicalProfessionAvailabilityRequests
                    .Include(x => x.Request)
                        .ThenInclude(r => r.ImagesProblemRequests)
                    .Where(x => x.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.UserId == UserId
                             && x.Request.StateRequestId == Constants.StateRequests.PENDING)
                    .Select(x => x.Request)
                    .ToListAsync();
            }
        }
    }
}
