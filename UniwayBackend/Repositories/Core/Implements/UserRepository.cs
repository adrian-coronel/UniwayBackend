using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using static UniwayBackend.Config.Constants;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
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
