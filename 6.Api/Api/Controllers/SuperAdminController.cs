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

[PermissionAuthorize]
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
        var rolesWithPermissions = await _superAdminServices.GetAllRolesWithPermissions();
        List<RoleDto> result = new();
        //ToDo: do this with auto mapper 
        rolesWithPermissions.ToList().ForEach(pair =>
            result.Add(new RoleDto(pair.Key.Id, pair.Key.Name)
                { Permissions = pair.Value.Select(claim => claim.Value).ToList() }));
        return result;
    }

    public async Task<ActionResult<List<UserDto>>> GetAllUsersWithRolesAndPermissions()
    {
        
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string name)
    {
        return Ok(await _superAdminServices.CreateRole(new IdentityRole<long>() { Name = name }));
    }

    [HttpPost]
    //ToDo: change the parameters to RoleDto
    public async Task<IActionResult> AddRolePermission(long roleId, IEnumerable<string> claimNames)
    {
        var claims = new List<Claim>();
        var permissionsUrls = ConvertRoutesToUrls(_routeDetector.GetAllRoutes(Assembly.GetExecutingAssembly()));
        foreach (var claim in claimNames)
        {
            //ToDo: this must be converted to fluent validation
            if (!permissionsUrls.Contains(claim,StringComparer.OrdinalIgnoreCase))
                return BadRequest("one of the Permissions is incorrect");

            claims.Add(new Claim("Permission", claim));
        }

        var result = await _superAdminServices.AddRolePermission(roleId, claims);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddUserPermissionsAndRoles(UserDto user)
    {
        return Ok();
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
                urls.Add($"api/{route.Key}/{actionName}");
            }
        }

        return urls;
    }
}