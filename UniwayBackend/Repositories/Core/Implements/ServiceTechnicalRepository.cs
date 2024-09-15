using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class ServiceTechnicalRepository : BaseRepository<ServiceTechnical, int>, IServiceTechnicalRepository
    {
        public async Task<List<ServiceTechnical>> FindAllByTechnicalProfessionAvailabilityId(int TechnicalProfessionAvailabilityId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.ServiceTechnicals
                    .Where(st => st.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId)
                    .ToListAsync();
            }
        }
    }
}
