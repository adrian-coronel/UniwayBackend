using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UniwayBackend.Config;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using static UniwayBackend.Config.Constants;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public async Task<List<User>> FindByAvailabilityAndLocation(Point point, short availabilityId = 0, int distance = 0)
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
                                      x.TechnicalProfession.UserTechnical.Technical.Location.Distance(point) <= distance &&
                                      x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == Constants.State.ACTIVE_BOOL &&

                                      !(context.Requests
                                        .Any(r => r.TechnicalProfessionAvailability.Id == x.Id && 
                                                  r.StateRequestId == Constants.StateRequests.IN_PROCESS)
                                       )
                                 )
                                 ||
                                 (
                                      availabilityId == Constants.Availabilities.IN_WORKSHOP_ID || availabilityId == Constants.Availabilities.BOTH_ID &&
                                      x.Workshops.Any(w => w.Location != null && w.Location.Distance(point) <= distance) &&
                                      x.TechnicalProfession.UserTechnical.Technical.WorkingStatus == Constants.State.ACTIVE_BOOL
                                 )
                             )
                    )
                    .Select(x => x.TechnicalProfession.UserTechnical.User)
                    .ToListAsync();


            }
        }

        public async Task<User?> FindByIdAndRoleId(Guid Id, short RoleId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<User>()
                    .SingleOrDefaultAsync(u => u.Id == Id && u.RoleId == RoleId);
            }
        }

        public async Task<User?> FindByTechnicalProfessionAvailabilityId(int technicalProfessionAvailabilityId)
        {
            using (var context = new DBContext())
            {
                var query = from user in context.Users
                            join userTechnical in context.UserTechnicals 
                                on user.Id equals userTechnical.UserId
                            join technicalProfession in context.TechnicalProfessions 
                                on userTechnical.Id equals technicalProfession.UserTechnicalId
                            join technicalProfessionAvailability in context.TechnicalProfessionAvailabilities
                                on technicalProfession.Id equals technicalProfessionAvailability.TechnicalProfessionId
                            where technicalProfessionAvailability.Id == technicalProfessionAvailabilityId
                            select user;

                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<User?> FindByUsernameAndPassword(string Email, string Password)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<User>()
                    .SingleOrDefaultAsync(u => u.Email == Email && u.Password == Password);
            }
        }
    }
}
