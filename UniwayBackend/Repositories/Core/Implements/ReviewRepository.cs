using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class ReviewRepository : BaseRepository<Review, int>, IReviewRepository
    {
        public async Task<List<Review>> FindAllByTechnicalId(int TechnicalId)
        {
            using (var context = new DBContext())
            {
                return await context.Reviews
                    .Where(x => x.TechnicalId == TechnicalId)
                    .ToListAsync();
            }
        }

        
    }
}
