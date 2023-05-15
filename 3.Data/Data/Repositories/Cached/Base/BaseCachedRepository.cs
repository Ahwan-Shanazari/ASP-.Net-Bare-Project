using System.Linq.Expressions;
using Data.Repositories.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories.Cached.Base;

public class BaseCachedRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly IBaseRepository<TEntity> _baseRepository;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    protected BaseCachedRepository(IBaseRepository<TEntity> baseRepository, IMemoryCache cache,IConfiguration configuration)
    {
        _baseRepository = baseRepository;
        _cache = cache;
        _configuration = configuration;
        CacheKey = _baseRepository.CacheKey;
    }

    public string CacheKey { get; }

    public async Task<TEntity> Read(Expression<Func<TEntity, bool>> predict)
    {
        return await _baseRepository.Read(predict);
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        var result = await _baseRepository.Create(entity);
        SetCache();
        return result;
    }

    public TEntity Update(TEntity entity)
    {
        var result = _baseRepository.Update(entity);
        SetCache();
        return result;
    }

    public bool Delete(TEntity entity)
    {
        var result = _baseRepository.Delete(entity);
        SetCache();
        return result;
    }

    public async Task<List<TEntity>> ReadAllFromCacheOrDb()
    {
        return await _baseRepository.ReadAllFromCacheOrDb();
    }

    public List<TEntity> ReadAll()
    {
        return _baseRepository.ReadAll();
    }

    private void SetCache()
    {
        var list = ReadAll();
        _cache.Set(CacheKey, list, new MemoryCacheEntryOptions()
        {
            Priority = CacheItemPriority.Normal,
            Size = list.Count,
            //ToDo: use a custom configuration class instead of the appsettings.json configuration  
            SlidingExpiration = TimeSpan.FromSeconds(int.Parse(_configuration["CacheOptions:SlidingExpSeconds"])),
            AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(int.Parse(_configuration["CacheOptions:AbsoluteExpSeconds"])),
        });
    }
}