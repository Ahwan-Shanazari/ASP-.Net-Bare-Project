using System.Linq.Expressions;
using Common;

namespace Data.Repositories.Base;

public interface IBaseRepository<TEntity> where TEntity : class
{
    string CacheKey { get; }
    Task<TEntity> Read(Expression<Func<TEntity, bool>> predict);
    Task<TEntity> Create(TEntity entity);
    TEntity Update(TEntity entity);
    bool Delete(TEntity entity);
    Task<List<TEntity>> ReadAllFromCacheOrDb();
    List<TEntity> ReadAll();
}