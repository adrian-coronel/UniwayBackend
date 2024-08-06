using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

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
