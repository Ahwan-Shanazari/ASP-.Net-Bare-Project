using Common;
using Data.Contexts;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;

    public UnitOfWork(DataContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public int CommitChanges()
    {
        return _context.SaveChanges();
    }
    
    public int CommitChanges(CacheKey key)
    {
        _cache.Remove(key.ToString());
        return _context.SaveChanges();
    }

    public async Task<int> CommitChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> CommitChangesAsync(CacheKey key)
    {
        _cache.Remove(key.ToString());
        return await _context.SaveChangesAsync();
    }
    
}