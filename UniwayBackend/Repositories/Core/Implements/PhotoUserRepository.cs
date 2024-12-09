using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class PhotoUserRepository : BaseRepository<PhotoUser, int>, IPhotoUserRepository
    {
        public async Task<PhotoUser?> FindByUserId(Guid UserId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.PhotoUsers
                    .Include(x => x.User)
                    .Where(x => x.UserId == UserId)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
