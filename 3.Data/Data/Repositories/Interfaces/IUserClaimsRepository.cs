using System.Security.Claims;

namespace Data.Repositories.Interfaces;

public interface IUserClaimsRepository
{
    List<Claim> GetUserPermissions(long userId);
}