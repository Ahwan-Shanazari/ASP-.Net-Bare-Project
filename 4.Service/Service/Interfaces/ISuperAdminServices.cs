using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Service.Interfaces;

public interface ISuperAdminServices
{
    Task<IDictionary<IdentityRole<long>, List<Claim>>> GetRolesWithPermissions(long? userId=null);
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
    Task<IDictionary<IdentityUser<long>, List<Claim>>> GetUsersWithPermissions();
    Task<bool> AddRolesToUser(long userId, List<string> roleNames);
    Task<bool> RemoveUserRoles(long userId,List<string>? roleName = null);
    Task<bool> UpdateUserSecurityStampAsync(long userId);
    Task<List<IdentityUser<long>>> GetAllUsers();
}