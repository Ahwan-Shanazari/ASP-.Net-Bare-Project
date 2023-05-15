using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.Repositories;

public class UserClaimsRepository:BaseRepository<IdentityUserClaim<long>>, IUserClaimsRepository
{
    public UserClaimsRepository(DataContext context, IMemoryCache cache, IConfiguration configuration) : base(context,cache, configuration)
    {
    }

    public List<Claim> GetUserPermissions(long userId)
    {
        return Entities.Where(userClaim => userClaim.UserId == userId && userClaim.ClaimType == "Permission")
            .Select(userClaim => userClaim.ToClaim()).ToList();
    }
}