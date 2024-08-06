
using Microsoft.EntityFrameworkCore;
using UniwayBackend.Context;

namespace UniwayBackend.Repositories.Base
{
    //  where TEntity : class => define que TEntity representará una entidad y no es tomado como un valor
    public class BaseRepository<TEntity, Key> : IBaseRepository<TEntity, Key> where TEntity : class
    {
        public async Task<List<TEntity>> FindAll()
        {
            // using => permite para crear instancias que solo existirán dentro del bloque
            using (DBContext context = new DBContext())
            {
                return await context.Set<TEntity>().ToListAsync();
            }
        }

        public async Task<TEntity?> FindById(Key Id)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Set<TEntity>().FindAsync(Id);
            }
        }
        

        public async Task<bool> Insert(TEntity entity)
        {
            using (DBContext context = new DBContext())
            {
                await context.AddAsync(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }
        public async Task<TEntity> InsertAndReturn(TEntity entity)
        {
            using (DBContext context = new DBContext())
            {
                await context.AddAsync(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Update(TEntity entity)
        {
            using (DBContext context = new DBContext())
            {
                context.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }
        public async Task<TEntity> UpdateAndReturn(TEntity entity)
        {
            using (DBContext context = new DBContext())
            {
                context.Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(Key Id)
        {
            using (DBContext context = new DBContext())
            {
                TEntity? entity = await context.Set<TEntity>().FindAsync(Id);

                if (entity != null)
                {
                    context.Remove(entity);
                    return await context.SaveChangesAsync() > 0;
                }

                return false;
            }
        }
    }
}
