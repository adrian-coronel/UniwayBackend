using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class TechnicalProfessionRepository : BaseRepository<TechnicalProfession, int>, ITechnicalProfessionRepository
    {
        public async Task<TechnicalProfession?> FindByTechnicalId(int technicalId)
        {
            using (var context = new DBContext())
            {
                return await context.TechnicalProfessions
                    .Where(x => x.UserTechnical.TechnicalId == technicalId)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
