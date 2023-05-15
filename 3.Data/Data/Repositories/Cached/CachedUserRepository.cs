using System.Linq.Expressions;
using Data.Repositories.Base;
using Data.Repositories.Cached.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories.Cached;

public class CachedUserRepository : BaseCachedRepository<IdentityUser<long>>, IUserRepository
{
    private readonly IUserRepository _userRepository;

    public CachedUserRepository(IUserRepository userRepository, IMemoryCache cache, IConfiguration configuration) :
        base(userRepository, cache, configuration)
    {
        _userRepository = userRepository;
    }
}