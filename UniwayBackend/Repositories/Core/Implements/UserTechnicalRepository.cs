using Microsoft.EntityFrameworkCore;
using System.Net;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;
using static UniwayBackend.Config.Constants;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class UserTechnicalRepository : BaseRepository<UserTechnical, int>, IUserTechnicalRepository
    {
        public async Task<bool> ExistsTechnicalByDni(string Dni)
        {
            using(DBContext context = new DBContext())
            {
                return await context.Set<Technical>()
                    .AnyAsync(t => t.Dni == Dni);
            }
        }

        public async Task<bool> ExistsUserTypeWithTechnicalByDni(short RoleId, string Dni)
        {
            using(DBContext context = new DBContext())
            {
                return await context.Set<UserTechnical>()
                    .AnyAsync(ut => ut.User.RoleId == RoleId && 
                                    ut.Technical.Dni == Dni &&
                                    ut.Enabled);
            }
        }

        public async Task<UserTechnical?> FindByUserIdAndRoleId(Guid userId, int roleId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<UserTechnical>()
                    .Include(x => x.User)
                    .Include(x => x.Technical)
                    .SingleOrDefaultAsync(x => x.User.Id == userId && x.User.RoleId == roleId);
            }
        }

        public async Task<Technical?> FindTechnicalByDni(string Dni)
        {
            using(DBContext context = new DBContext())
            {
                return await context.Set<Technical>()
                    .SingleOrDefaultAsync(t => t.Dni == Dni);
            }
        }
    }
}
