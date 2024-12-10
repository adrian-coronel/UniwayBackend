using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalProfessionAvailabilityRepository : BaseRepository<TechnicalProfessionAvailability, int>, ITechnicalProfessionAvailabilityRepository
    {
        public async Task<List<TechnicalProfessionAvailability>> FindByAvailabilityAndLocation(Point point, short availabilityId = 0, int distance = 5000)
        {
            using (var context = new DBContext())
            {
                return await context.Set<TechnicalProfessionAvailability>()
                    .Where(x =>
                        (
                            // Condiciones para AT_HOME o BOTH
                            (availabilityId == Constants.Availabilities.AT_HOME_ID || availabilityId == Constants.Availabilities.BOTH_ID)
                            &&
                            x.TechnicalProfession.UserTechnical.Technical.Location != null
                            && x.TechnicalProfession.UserTechnical.Technical.Location.Distance(point) <= distance
                            && x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == Constants.State.ACTIVE_BOOL
                            && !(context.Requests
                                   .Any(r => r.TechnicalProfessionAvailability.Id == x.Id
                                             && r.StateRequestId == Constants.StateRequests.IN_PROCESS)
                               )
                        )
                        ||
                        (
                            // Condiciones para IN_WORKSHOP o BOTH
                            (availabilityId == Constants.Availabilities.IN_WORKSHOP_ID || availabilityId == Constants.Availabilities.BOTH_ID)
                            &&
                            x.Workshops.Any(w => w.Location != null && w.Location.Distance(point) <= distance)
                            && x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == Constants.State.ACTIVE_BOOL
                        )
                    )
                    .Distinct()
                    .ToListAsync();


            }
        }

        public async Task<TechnicalProfessionAvailability?> FindByTechnicalAndAvailability(int technicalId, short availabilityId)
        {
            using (var context = new DBContext())
            {
                return await context.Set<TechnicalProfessionAvailability>()
                    .FirstOrDefaultAsync(x => x.AvailabilityId == availabilityId 
                                           && x.TechnicalProfession.UserTechnical.TechnicalId == technicalId);
            }
        }

        public async Task<TechnicalProfessionAvailability?> FindTechnicalInformationByTechnicalProfeessionAvailabilityId(int TechnicalProfessionAvailabilityId)
        {
            using (var context = new DBContext())
            {
                return await context.Set<TechnicalProfessionAvailability>()
                    .Include(x => x.TechnicalProfession)
                        .ThenInclude(y => y.UserTechnical)
                    .FirstOrDefaultAsync(x => x.Id == TechnicalProfessionAvailabilityId);
            }

        }
    }
}
