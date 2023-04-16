using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service;

public class SuperAdminServices : ISuperAdminServices
{
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly UserManager<IdentityUser<long>> _userManager;

    public SuperAdminServices(RoleManager<IdentityRole<long>> roleManager, UserManager<IdentityUser<long>> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IDictionary<IdentityRole<long>, List<Claim>>> GetAllRolesWithPermissions()
    {
        Dictionary<IdentityRole<long>, List<Claim>> result = new();
        var roles = await _roleManager.Roles.ToListAsync();
        foreach (var role in roles)
        {
            List<Claim> rolePermissions = (await _roleManager.GetClaimsAsync(role)).Where(claim => claim.Type.Equals("Permission"))
                .Select(claim => claim).ToList();
            //ToDo: check what will happen if rolePermissions was null
            result.Add(role, rolePermissions ?? new List<Claim>());
        }
        
        return result;
    }

    public async Task<Tuple<IdentityUser<long>,Tuple<List<IdentityRole<long>>>,List<Claim>>> GetAllUsersWithRolesAndPermissions()
    {
        foreach (var user in await _userManager.Users.ToListAsync())
        {
            List<Claim> claims = new();
            var c2laims = await _userManager.GetClaimsAsync(user);
        }
    }

    public async Task<bool> CreateRole(IdentityRole<long> role)
    {
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
            return true;
        return false;
    }
    
    //ToDo: we must create a logic for AddPermission methods to not add again an existing permission    
    
    public async Task<bool> AddRolePermission(long roleId, IEnumerable<Claim> claims)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        foreach (var claim in claims.Where(claim => claim.Type.Equals("Permission")))
        {
            if (!(await _roleManager.AddClaimAsync(role, claim)).Succeeded)
                return false;
        }

        return true;
    }

    public async Task<bool> AddRolePermission(long roleId, Claim claim)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (claim.Type.Equals("Permission"))
            return (await _roleManager.AddClaimAsync(role, claim)).Succeeded;
        return false;
    }

    public async Task<bool> AddUserPermission(long userId, IEnumerable<Claim> claims)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return (await _userManager.AddClaimsAsync(user, claims.Where(claim => claim.Type.Equals("Permission"))))
            .Succeeded;
    }

    public async Task<bool> AddUserPermission(long userId, Claim claim)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (claim.Type.Equals("Permission"))
            return (await _userManager.AddClaimAsync(user, claim))
                .Succeeded;
        return false;
    }

    public async Task<bool> AddControllerPermissionToRole(long roleId, string controller,
        Dictionary<string, List<string>> allRoutes)
    {
        if (allRoutes.Keys.Contains(controller))
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            foreach (var action in allRoutes.FirstOrDefault(pair => pair.Key.Equals(controller)).Value)
            {
                if (!(await _roleManager.AddClaimAsync(role, new Claim("Permission", $"api/{controller}/{action}")))
                    .Succeeded)
                    return false;
            }

            return true;
        }

        return false;
    }

    public async Task<bool> AddControllerPermissionToUser(long userId, string controller,
        Dictionary<string, List<string>> allRoutes)
    {
        if (allRoutes.Keys.Contains(controller))
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            foreach (var action in allRoutes.FirstOrDefault(pair => pair.Key.Equals(controller)).Value)
            {
                if (!(await _userManager.AddClaimAsync(user, new Claim("Permission", $"api/{controller}/{action}")))
                    .Succeeded)
                    return false;
            }

            return true;
        }

        return false;
    }

    public async Task<bool> AddControllersPermissionToRole(long roleId, List<string> controllers,
        Dictionary<string, List<string>> allRoutes)
    {
        foreach (var controller in controllers)
        {
            if (!(await AddControllerPermissionToRole(roleId, controller, allRoutes)))
                return false;
        }

        return true;
    }

    public async Task<bool> AddControllersPermissionToUser(long userId, List<string> controllers,
        Dictionary<string, List<string>> allRoutes)
    {
        foreach (var controller in controllers)
        {
            if (!(await AddControllerPermissionToUser(userId, controller, allRoutes)))
                return false;
        }

        return true;
    }

    public async Task<bool> DeleteUserPermissions(long userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var userClaims = (await _userManager.GetClaimsAsync(user)).Where(claim => claim.Type.Equals("Permission"));
        return (await _userManager.RemoveClaimsAsync(user, userClaims)).Succeeded;
    }

    public async Task<bool> DeleteRolePermissions(long roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        var roleClaims = (await _roleManager.GetClaimsAsync(role)).Where(claim => claim.Type.Equals("Permission"));
        foreach (var claim in roleClaims)
        {
            if (!(await _roleManager.RemoveClaimAsync(role, claim)).Succeeded)
                return false;
        }

        return true;

    }
}