using System.Linq.Expressions;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Base;

public abstract class BaseRepository<TEntity>:IBaseRepository<TEntity> where TEntity : class
{
    private readonly IMemoryCache _cache;
    protected readonly IConfiguration Configuration;
    public string CacheKey { get; }
    protected readonly DbSet<TEntity> Entities;
    protected BaseRepository(DataContext context, IMemoryCache cache, IConfiguration configuration)
    {
        _cache = cache;
        Configuration = configuration;
        CacheKey = typeof(TEntity).Name.Replace("`1","");
        Entities = context.Set<TEntity>();
    }

    public async Task<TEntity> Read(Expression<Func<TEntity,bool>> predict)
    {
        return await Entities.AsNoTracking().FirstOrDefaultAsync(predict);
    }
    
    public List<TEntity> ReadAll()
    {
        return Entities.ToList();
    }
    
    public async Task<List<TEntity>> ReadAllFromCacheOrDb()
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            //ToDo:This Information Is General And Must Be Read From App Settings Or By Option Pattern
            var entities = await Entities.AsNoTracking().ToListAsync(); 
            entry.Priority = CacheItemPriority.Normal;
            entry.Size = entities.Count;
            entry.SlidingExpiration = TimeSpan.FromSeconds(int.Parse(Configuration["CacheOptions:SlidingExpSeconds"]));
            entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(int.Parse(Configuration["CacheOptions:AbsoluteExpSeconds"]));
            return entities;
        } );
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        _cache.Remove(CacheKey);
        return (await Entities.AddAsync(entity)).Entity;
    }

    public TEntity Update(TEntity entity)
    {
        _cache.Remove(CacheKey);
        return Entities.Update(entity).Entity;
    }

    public bool Delete(TEntity entity)
    {
        if (!Entities.Contains(entity))
            return false;
        
        Entities.Remove(entity);
        _cache.Remove(CacheKey);
        
        return true;
    }
}