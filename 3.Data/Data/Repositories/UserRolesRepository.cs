using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories;

public class UserRolesRepository:BaseRepository<IdentityUserRole<long>>, IUserRolesRepository
{
    public UserRolesRepository(DataContext context, IMemoryCache cache, IConfiguration configuration) : base(context,cache, configuration)
    {
    }

    public List<long> GetUserRoleIds(long userId)
    {
        return Entities.Where(userRole => userRole.UserId == userId).Select(userRole => userRole.RoleId).ToList();
    }
}