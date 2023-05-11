using System.Security.Claims;

namespace Data.Repositories.Interfaces;

public interface IRoleClaimRepository
{
    List<Claim> GetRoleClaims(long roleId);
}