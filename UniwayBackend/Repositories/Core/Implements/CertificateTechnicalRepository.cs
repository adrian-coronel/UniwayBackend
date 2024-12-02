using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;
using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Interfaces;

namespace UniwayBackend.Repositories.Core.Implements
{
    public class CertificateTechnicalRepository : BaseRepository<CertificateTechnical, int>, ICertificateTechnicalRepository
    {
        public async Task<CertificateTechnical?> GetByFileName(string filename)
        {
            using(DBContext context = new DBContext())
            {
                return await context.CertificateTechnicals
                    .Where(x => x.OriginalName.Contains(filename))
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<CertificateTechnical?> GetByTechnicalId(int TechnicalId)
        {
            using (DBContext context = new DBContext())
            {
                return await context.CertificateTechnicals
                    .Where(x => x.TechnicalId == TechnicalId)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
