using UniwayBackend.Models.Entities;

namespace UniwayBackend.Repositories.Base
{
    public interface IBaseRepository<TEntity, Key>
    {
        Task<List<TEntity>> FindAll();
        Task<TEntity?> FindById(Key Id);
        Task<bool> Insert(TEntity entity);
        Task<TEntity> InsertAndReturn(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<TEntity> UpdateAndReturn(TEntity entity);
        Task<bool> Delete(Key Id);
    }
}
