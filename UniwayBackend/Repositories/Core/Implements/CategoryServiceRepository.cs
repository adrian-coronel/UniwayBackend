using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class CategoryServiceRepository : BaseRepository<CategoryService, short>, ICategoryServiceRepository
    {
        public async Task<List<CategoryService>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.CategoryServices
                    .Include(x => x.ServiceTechnicals)
                    .Where(cs => cs.ServiceTechnicals.Any(st => st.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId))
                    .ToListAsync();
            }
        }


        public async Task<CategoryService?> FindByIdAndTechnicalProfessionAvailabilityId(short CategoryServiceId, int TechnicalProfessionAvailabilityId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.CategoryServices
                    .Include(x => x.ServiceTechnicals)
                    .FirstOrDefaultAsync(cs => cs.Id == CategoryServiceId &&
                                               cs.ServiceTechnicals
                                                    .Any(st => st.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId));
                    
            }
        }
    }
}
