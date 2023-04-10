using System.Linq.Expressions;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Repositories.Base;

public class BaseRepository<TEntity>:IBaseRepository<TEntity> where TEntity : class
{
    public readonly DbSet<TEntity> entities;

    public BaseRepository(DataContext context)
    {
        entities = context.Set<TEntity>();
    }

    public async Task<TEntity> Read(Expression<Func<TEntity,bool>> predict)
    {
        return await entities.FirstOrDefaultAsync(predict);
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        return (await entities.AddAsync(entity)).Entity;
    }

    public TEntity Update(TEntity entity)
    {
        return entities.Update(entity).Entity;
    }

    public bool Delete(TEntity entity)
    {
        if (!entities.Contains(entity))
            return false;
        entities.Remove(entity);
        return true;
    }
}