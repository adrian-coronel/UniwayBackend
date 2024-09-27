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
        public async Task<List<TechnicalProfessionAvailability>> FindByAvailabilityAndLocation(short availabilityId, Point point, int distance)
        {
            using (var context = new DBContext())
            {
                return await context.Set<TechnicalProfessionAvailability>()
                    .Where(x => (x.AvailabilityId == availabilityId || availabilityId == Constants.Availabilities.BOTH_ID)
                             &&
                             (
                                 (
                                    availabilityId == Constants.Availabilities.AT_HOME_ID || availabilityId == Constants.Availabilities.BOTH_ID &&
                                      x.TechnicalProfession.UserTechnical.Technical.Location != null &&
                                      x.TechnicalProfession.UserTechnical.Technical.Location.Distance(point) <= distance
                                 )
                                 ||
                                 (
                                    availabilityId == Constants.Availabilities.IN_WORKSHOP_ID || availabilityId == Constants.Availabilities.BOTH_ID &&
                                      x.Workshops.Any(w => w.Location != null && w.Location.Distance(point) <= distance)
                                 )
                             )
                    ).ToListAsync();


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
    }
}
