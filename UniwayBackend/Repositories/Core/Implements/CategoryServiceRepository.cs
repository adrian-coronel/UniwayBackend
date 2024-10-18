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

        public async Task<List<CategoryService>> FindByIdAndAvailabilityId(short? CategoryServiceId = null, int? TechnicalProfessionAvailabilityId = null, short? AvailablityId = null)
        {
            using (DBContext context = new DBContext())
            {
                return await context.CategoryServices
                    .Include(x => x.ServiceTechnicals)
                        .ThenInclude(s => s.Images)
                    .Include(x => x.ServiceTechnicals)
                        .ThenInclude(s => s.TechnicalProfessionAvailability)
                            .ThenInclude(tpa => tpa.Availability)
                    .Where(cs => (CategoryServiceId == null || cs.Id == CategoryServiceId) &&
                                 (TechnicalProfessionAvailabilityId == null || cs.ServiceTechnicals.Any(s => s.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId)) &&
                                 (AvailablityId == null || cs.ServiceTechnicals.Any(s => s.TechnicalProfessionAvailability.AvailabilityId == AvailablityId))
                           )
                    .ToListAsync();

            }
        }
    }
}
