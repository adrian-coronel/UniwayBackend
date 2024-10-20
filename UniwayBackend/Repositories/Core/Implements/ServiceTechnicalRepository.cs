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
                    .Include(x => x.ServiceTechnicalTypeCars)
                        .ThenInclude(x => x.TypeCar)
                    .Where(st => st.TechnicalProfessionAvailabilityId == TechnicalProfessionAvailabilityId)
                    .ToListAsync();
            }
        }

        public async Task<List<ServiceTechnical>> FindByTechnicalIdAndAvailabilityId(int TechnicalId, short AvailabilityId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.ServiceTechnicals
                    .Include(x => x.ServiceTechnicalTypeCars)
                        .ThenInclude(x => x.TypeCar)
                    .Where(st => st.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.TechnicalId == TechnicalId &&
                                 (AvailabilityId == 0 || st.TechnicalProfessionAvailability.AvailabilityId == AvailabilityId)
                          )
                    .ToListAsync();

            }
        }

        public async Task<List<ServiceTechnical>> FindFiveByTechnicalId(int TechnicalId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.ServiceTechnicals
                    .Where(st => st.TechnicalProfessionAvailability.TechnicalProfession.UserTechnical.TechnicalId == TechnicalId)
                    .Take(5)
                    .ToListAsync();

            }
        }

        public async Task<List<ServiceTechnical>> FindFiveByWorkshopId(int WorkshopId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.ServiceTechnicals
                    .Where(st => st.TechnicalProfessionAvailability.Workshops.Any(x => x.Id == WorkshopId))
                    .Take(5)
                    .ToListAsync();

            }
        }

        public async Task<ServiceTechnical?> GetByIdWithInformation(int ServiceTechnicalId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.ServiceTechnicals
                    .Include(x => x.ServiceTechnicalTypeCars)
                        .ThenInclude(sttc => sttc.TypeCar)
                    .Include(x => x.TechnicalProfessionAvailability)
                        .ThenInclude(tpa => tpa.Workshops)
                    .Include(x => x.TechnicalProfessionAvailability)
                        .ThenInclude(x => x.TechnicalProfession)
                            .ThenInclude(x => x.UserTechnical)
                                .ThenInclude(x => x.Technical)
                    .FirstOrDefaultAsync(x => x.Id == ServiceTechnicalId);
            }
        }
    }
}
