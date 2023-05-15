using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories;

public class RoleClaimRepository : BaseRepository<IdentityRoleClaim<long>>, IRoleClaimRepository
{
    public RoleClaimRepository(DataContext context, IMemoryCache cache, IConfiguration configuration) : base(context,cache, configuration)
    {
    }

    public List<Claim> GetRoleClaims(long roleId)
    {
        return Entities.Where(roleClaim => roleClaim.RoleId == roleId && roleClaim.ClaimType == "Permission")
            .Select(roleClaim => roleClaim.ToClaim()).ToList();
    }
}