using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Service.Interfaces;

public interface ISuperAdminServices
{
    Task<IDictionary<IdentityRole<long>, List<Claim>>> GetAllRolesWithPermissions();
    Task<bool> CreateRole(IdentityRole<long> role);
    Task<bool> AddRolePermission(long roleId, IEnumerable<Claim> claims);
    Task<bool> AddRolePermission(long roleId, Claim claim);
    Task<bool> AddUserPermission(long userId, IEnumerable<Claim> claims);
    Task<bool> AddUserPermission(long userId, Claim claim);

    Task<bool> AddControllerPermissionToRole(long roleId, string controller,
        Dictionary<string, List<string>> allRoutes);

    Task<bool> AddControllerPermissionToUser(long userId, string controller,
        Dictionary<string, List<string>> allRoutes);

    Task<bool> AddControllersPermissionToRole(long roleId, List<string> controllers,
        Dictionary<string, List<string>> allRoutes);

    Task<bool> AddControllersPermissionToUser(long userId, List<string> controllers,
        Dictionary<string, List<string>> allRoutes);

    Task<bool> DeleteUserPermissions(long userId);
    Task<bool> DeleteRolePermissions(long roleId);
}