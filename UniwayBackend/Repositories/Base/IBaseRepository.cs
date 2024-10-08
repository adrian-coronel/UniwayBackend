using UniwayBackend.Models.Entities;

namespace UniwayBackend.Repositories.Base
{
    public interface IBaseRepository<TEntity, Key>
    {
        Task<List<TEntity>> FindAll();
        Task<bool> ExistById(Key Id);
        Task<TEntity?> FindById(Key Id);
        Task<bool> Insert(TEntity entity);
        Task<List<TEntity>> InsertAll(List<TEntity> entities);
        Task<TEntity> InsertAndReturn(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<TEntity> UpdateAndReturn(TEntity entity);
        Task<bool> Delete(Key Id);
    }
}
