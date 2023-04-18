using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public class RoleClaimRepository : BaseRepository<IdentityRoleClaim<long>>, IRoleClaimRepository
{
    public RoleClaimRepository(DataContext context) : base(context)
    {
    }

    public List<Claim> GetRoleClaims(long roleId)
    {
        return entities.Where(roleClaim => roleClaim.RoleId == roleId && roleClaim.ClaimType == "Permission")
            .Select(roleClaim => roleClaim.ToClaim()).ToList();
    }
}