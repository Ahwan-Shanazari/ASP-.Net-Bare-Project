using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

public class RoleClaimRepository : BaseRepository<IdentityRoleClaim<long>>, IRoleClaimRepository
{
    public RoleClaimRepository(DataContext context,IMemoryCache cache) : base(context,cache)
    {
    }

    public List<Claim> GetRoleClaims(long roleId)
    {
        return Entities.Where(roleClaim => roleClaim.RoleId == roleId && roleClaim.ClaimType == "Permission")
            .Select(roleClaim => roleClaim.ToClaim()).ToList();
    }
}