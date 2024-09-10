using UniwayBackend.Models.Entities;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Repositories.Core.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review, int>
    {

        Task<List<Review>> FindAllByTechnicalId(int TechnicalId);


    }
}
