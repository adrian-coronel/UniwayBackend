using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class ClientRepository : BaseRepository<Client, int>, IClientRepository
    {
        public async Task<Client> FindByIdIncludeData(int id)
        {
            using (var context = new DBContext())
            {
                return await context.Set<Client>()
                    .Include(x => x.User)
                        .ThenInclude(u => u.PhotoUser)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Client?> FindByUserId(Guid UserId)
        {
            using (var context = new DBContext())
            {
                return await context.Set<Client>()
                    .Include(x => x.User)
                        .ThenInclude(u => u.PhotoUser)
                    .SingleOrDefaultAsync(x => x.UserId == UserId);
            }
        }
    }
}
