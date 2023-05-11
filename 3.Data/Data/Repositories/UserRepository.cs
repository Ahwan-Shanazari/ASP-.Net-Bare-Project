using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

public class UserRepository:BaseRepository<IdentityUser<long>>, IUserRepository
{
    public UserRepository(DataContext context,IMemoryCache cache) : base(context,cache)
    {
    }
    
}