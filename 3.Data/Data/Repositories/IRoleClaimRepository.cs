using System.Security.Claims;

namespace Data.Repositories;

public interface IRoleClaimRepository
{
    List<Claim> GetRoleClaims(long roleId);
}