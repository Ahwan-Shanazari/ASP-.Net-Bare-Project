using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public interface IUserClaimsRepository
{
    List<Claim> GetUserPermissions(long userId);
}