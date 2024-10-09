using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class ImagesProblemRequestRepository : BaseRepository<ImagesProblemRequest, int>, IImagesProblemRequestRepository
    {
        public async Task<List<ImagesProblemRequest>> FindAllByRequestId(int RequestId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.ImagesProblemRequests
                    .Where(x => x.RequestId == RequestId)
                    .ToListAsync();
            }
        }
    }
}
