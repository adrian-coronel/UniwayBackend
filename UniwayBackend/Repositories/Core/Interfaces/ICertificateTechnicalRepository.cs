using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface ICertificateTechnicalRepository : IBaseRepository<CertificateTechnical, int>
    {
        Task<CertificateTechnical?> GetByFileName(string filename);
        Task<CertificateTechnical?> GetByTechnicalId(int TechnicalId);
    }
}
