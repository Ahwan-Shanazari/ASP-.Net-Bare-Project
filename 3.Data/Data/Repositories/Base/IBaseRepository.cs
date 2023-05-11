using System.Linq.Expressions;

namespace Data.Repositories.Base;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> Read(Expression<Func<TEntity,bool>> predict);
    Task<TEntity> Create(TEntity entity);
    TEntity Update(TEntity entity);
    bool Delete(TEntity entity);
    Task<List<TEntity>> ReadAllFromCacheOrDb();
}