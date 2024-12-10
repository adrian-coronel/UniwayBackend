using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class PhotoWorkshopRepository : BaseRepository<PhotoWorkshop, int>, IPhotoWorkshopRepository
    {
        public async Task<PhotoWorkshop?> FindByWorkshopId(int workshopId)
        {
            using (DBContext context = new DBContext())
            {

                return await context.PhotoWorkshops
                    .Where(w => w.WorkshopId == workshopId)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
