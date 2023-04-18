using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public class UserRolesRepository:BaseRepository<IdentityUserRole<long>>, IUserRolesRepository
{
    public UserRolesRepository(DataContext context) : base(context)
    {
    }

    public List<long> GetUserRoleIds(long userId)
    {
        return entities.Where(userRole => userRole.UserId == userId).Select(userRole => userRole.RoleId).ToList();
    }
}