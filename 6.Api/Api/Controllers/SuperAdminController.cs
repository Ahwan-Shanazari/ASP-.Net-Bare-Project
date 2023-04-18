using System.Collections;
using System.Reflection;
using System.Security.Claims;
using Api.Dtos;
using Api.Dtos.AdminDtos;
using AutoMapper;
using Framework;
using Framework.CustomAttributes;
using Framework.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

//[PermissionAuthorize]
[ServiceFilter(typeof(PermissionAuthorizeAttribute))]
public class SuperAdminController : BaseController
{
    private readonly IRouteDetector _routeDetector;
    private readonly ISuperAdminServices _superAdminServices;

    public SuperAdminController(IRouteDetector routeDetector, IMapper mapper, ISuperAdminServices superAdminServices)
        : base(mapper)
    {
        _routeDetector = routeDetector;
        _superAdminServices = superAdminServices;
    }

    [HttpGet]
    public IActionResult GetAllPermissions()
    {
        //return Ok(_routeDetector.GetAllRoutes(typeof(SuperAdminController).Assembly));
        return Ok(ConvertRoutesToUrls(_routeDetector.GetAllRoutes(Assembly.GetExecutingAssembly())));
    }

    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAllRolesWithPermissions()
    {
        var rolesWithPermissions = await _superAdminServices.GetRolesWithPermissions();
        List<RoleDto> result = new();
        //ToDo: do this with auto mapper 
        rolesWithPermissions.ToList().ForEach(pair =>
            result.Add(new RoleDto(pair.Key.Id, pair.Key.Name)
                { Permissions = pair.Value.Select(claim => claim.Value).ToList() }));
        return result;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsersWithRolesAndPermissions()
    {
        var usersWithPermissions = await _superAdminServices.GetUsersWithPermissions();
        //ToDo: do this with auto mapper
        List<UserDto> result = new();
        foreach (var pair in usersWithPermissions)
        {
            UserDto dto = new();
            dto.UserId = pair.Key.Id;
            dto.CustomPermissions = pair.Value.Select(claim => claim.Value).ToList();
            //ToDo:what will happen if we use user instead of userId and just pass these keys as parameters
            List<RoleDto> dtoRoles = new();
            dto.Roles =(await _superAdminServices.GetRolesWithPermissions(pair.Key.Id)).Select(rolePair =>
            {
                return new RoleDto(rolePair.Key.Id, rolePair.Key.Name)
                    { Permissions = rolePair.Value.Select(claim => claim.Value).ToList() };
            }).ToList();
            result.Add(dto);
        }

        return result;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string name)
    {
        return Ok(await _superAdminServices.CreateRole(new IdentityRole<long>() { Name = name }));
    }

    [HttpPost]
    public async Task<IActionResult> AddRolesToUser(long userId, List<string> roleNames)
    {
        return Ok(await _superAdminServices.AddRolesToUser(userId, roleNames));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveUserRoles(long userId)
    {
        return Ok(await _superAdminServices.RemoveUserRoles(userId));
    }

    [HttpPost]
    //ToDo: change the parameters to Dto?
    public async Task<IActionResult> AddRolePermission(long roleId, IEnumerable<string> claimNames)
    {
        var claims = new List<Claim>();
        //ToDo: Use Fluent validator on dto for this checking 
        var permissionsUrls = ConvertRoutesToUrls(_routeDetector.GetAllRoutes(Assembly.GetExecutingAssembly()));
        foreach (var claim in claimNames)
        {
            //ToDo: this must be converted to fluent validation
            if (!permissionsUrls.Contains(claim, StringComparer.OrdinalIgnoreCase))
                return BadRequest("one of the Permissions is incorrect");

            claims.Add(new Claim("Permission", claim));
        }

        var result = await _superAdminServices.AddRolePermission(roleId, claims);
        return Ok(result);
    }

    [HttpPost]
    //ToDo: change the parameters to Dto?
    public async Task<IActionResult> AddUserPermissions(long userId, List<string> permissions)
    {
        var claims = new List<Claim>();
        //ToDo: Use Fluent validator on dto for this checking 
        var permissionsUrls = ConvertRoutesToUrls(_routeDetector.GetAllRoutes(Assembly.GetExecutingAssembly()));
        foreach (var permission in permissions)
        {
            //ToDo: this must be converted to fluent validation
            if (!permissionsUrls.Contains(permission, StringComparer.OrdinalIgnoreCase))
                return BadRequest("one of the Permissions is incorrect");
            
            claims.Add(new Claim("Permission",permission));
        }

        return Ok(await _superAdminServices.AddUserPermission(userId,claims));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRolePermissions(long roleId)
    {
        var result = await _superAdminServices.DeleteRolePermissions(roleId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUserPermissions(long userId)
    {
        var result = await _superAdminServices.DeleteUserPermissions(userId);
        return Ok(result);
    }

    private List<string> ConvertRoutesToUrls(Dictionary<string, List<string>> allRoutes)
    {
        var urls = new List<string>();
        foreach (var route in allRoutes)
        {
            foreach (var actionName in route.Value)
            {
                urls.Add($"/api/{route.Key}/{actionName}");
            }
        }

        return urls;
    }
}