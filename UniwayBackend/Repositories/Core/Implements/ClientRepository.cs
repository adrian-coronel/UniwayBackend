using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class ClientRepository : BaseRepository<Client, int>, IClientRepository
    {
        public async Task<Client?> FindByUserId(Guid UserId)
        {
            using (var context = new DBContext())
            {
                return await context.Set<Client>()
                    .Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.UserId == UserId);
            }
        }
    }
}
