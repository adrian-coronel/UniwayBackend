using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

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
