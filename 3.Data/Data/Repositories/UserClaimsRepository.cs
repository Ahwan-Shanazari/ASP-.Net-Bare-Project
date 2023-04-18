using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public class UserClaimsRepository:BaseRepository<IdentityUserClaim<long>>, IUserClaimsRepository
{
    public UserClaimsRepository(DataContext context) : base(context)
    {
    }

    public List<Claim> GetUserPermissions(long userId)
    {
        return entities.Where(userClaim => userClaim.UserId == userId && userClaim.ClaimType == "Permissions")
            .Select(userClaim => userClaim.ToClaim()).ToList();
    }
}