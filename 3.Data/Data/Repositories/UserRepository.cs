using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories;

public class UserRepository:BaseRepository<IdentityUser<long>>, IUserRepository
{
    public UserRepository(DataContext context, IMemoryCache cache, IConfiguration configuration) : base(context,cache, configuration)
    {
    }
    
}