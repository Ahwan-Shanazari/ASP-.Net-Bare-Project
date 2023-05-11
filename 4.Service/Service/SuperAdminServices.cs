using System.Collections;
using System.Security.Claims;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service;

//ToDo: Create region for codes
public class SuperAdminServices : ISuperAdminServices
{
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly UserManager<IdentityUser<long>> _userManager;
    private readonly IUserRepository _userRepository;

    public SuperAdminServices(RoleManager<IdentityRole<long>> roleManager, UserManager<IdentityUser<long>> userManager, IUserRepository userRepository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<List<IdentityUser<long>>> GetAllUsers()
    {
        return await _userRepository.ReadAllFromCacheOrDb();
    }
    
    public async Task<IDictionary<IdentityRole<long>, List<Claim>>> GetRolesWithPermissions(long? userId=null)
    {
        Dictionary<IdentityRole<long>, List<Claim>> result = new();
        List<IdentityRole<long>> roles;
        if (userId is null)
            roles = await _roleManager.Roles.ToListAsync();
        else
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            roles = new();
            foreach (var roleName in await _userManager.GetRolesAsync(user))
            {
                roles.Add(await _roleManager.FindByNameAsync(roleName));
            }
        }
        foreach (var role in roles)
        {
            List<Claim> rolePermissions = (await _roleManager.GetClaimsAsync(role))
                .Where(claim => claim.Type.Equals("Permission")).ToList();
            //ToDo: check what will happen if rolePermissions was null
            result.Add(role, rolePermissions ?? new List<Claim>());
        }

        return result;
    }

    public async Task<IDictionary<IdentityUser<long>, List<Claim>>> GetUsersWithPermissions()
    {
        Dictionary<IdentityUser<long>, List<Claim>> result = new();
        foreach (var user in await _userManager.Users.ToListAsync())
        {
            List<Claim> userPermissions = (await _userManager.GetClaimsAsync(user))
                .Where(claim => claim.Type.Equals("Permission")).ToList();
            result.Add(user, userPermissions ?? new List<Claim>());
        }

        return result;
    }

    public async Task<bool> CreateRole(IdentityRole<long> role)
    {
        role.Name = role.Name.ToLower();
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
            return true;
        return false;
    }

    public async Task<bool> AddRolesToUser(long userId, List<string> roleNames)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return false;
        foreach (var roleName in roleNames)
        {
            if (!(await _userManager.AddToRoleAsync(user, roleName.ToLower())).Succeeded)
                return false;
        }
        return true;
    }

    public async Task<bool> RemoveUserRoles(long userId,List<string>? roleName = null)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null)
            return false;
        
        else if (roleName is not null)
        {
            //ToDo: implement logic here
        }
        else
        {
            var roles =await _userManager.GetRolesAsync(user);
            if (!(await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded)
                return false;
        }

        return true;
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
                if (!(await _roleManager.AddClaimAsync(role, new Claim("Permission", $"/api/{controller}/{action}")))
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
                if (!(await _userManager.AddClaimAsync(user, new Claim("Permission", $"/api/{controller}/{action}")))
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

    public async Task<bool> UpdateUserSecurityStampAsync(long userId)
    {
        var user =await _userManager.FindByIdAsync(userId.ToString());
        return (await _userManager.UpdateSecurityStampAsync(user)).Succeeded;
    }
}