using System.Linq.Expressions;
using Data.Repositories.Base;
using Data.Repositories.Cached.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories.Cached;

public class CachedUserRepository:BaseCachedRepository<IdentityUser<long>>,IUserRepository
{
    private readonly IUserRepository _userRepository;
    public CachedUserRepository(IUserRepository userRepository,IMemoryCache cache) : base(userRepository, cache)
    {
        _userRepository = userRepository;
    }
}