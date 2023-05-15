using System.Linq.Expressions;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories.Cached;

public class CachedUserRepository:IUserRepository
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;

    public CachedUserRepository(IUserRepository userRepository,IMemoryCache cache)
    {
        _userRepository = userRepository;
        _cache = cache;
        CacheKey = _userRepository.CacheKey;
    }

    public string CacheKey { get; }

    public async Task<IdentityUser<long>> Read(Expression<Func<IdentityUser<long>, bool>> predict)
    {
        
        return await _userRepository.Read(predict);
    }

    public async Task<IdentityUser<long>> Create(IdentityUser<long> entity)
    {
        var result = await _userRepository.Create(entity);
        SetCache();
        return result;
    }

    public IdentityUser<long> Update(IdentityUser<long> entity)
    {
        var result = _userRepository.Update(entity);
        SetCache();
        return result;
    }

    public bool Delete(IdentityUser<long> entity)
    {
        var result = _userRepository.Delete(entity);
        SetCache();
        return result;
    }

    public async Task<List<IdentityUser<long>>> ReadAllFromCacheOrDb()
    {
        return await _userRepository.ReadAllFromCacheOrDb();
    }

    public List<IdentityUser<long>> ReadAll()
    {
        return _userRepository.ReadAll();
    }

    private void SetCache()
    {
        var list = ReadAll();
        _cache.Set(CacheKey,list , new MemoryCacheEntryOptions()
        {
            Priority = CacheItemPriority.Normal,
            Size = list.Count,
            SlidingExpiration = TimeSpan.FromSeconds(30),
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1),
        });
    }
}