using System.Security.Claims;
using Data.Contexts;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.CustomAttributes;

//ToDo: must also implement super admin check from json or custom configuration class 
public class PermissionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IUserClaimsRepository _userClaimsRepository;
    private readonly IUserRolesRepository _userRolesRepository;
    private readonly IRoleClaimRepository _roleClaimRepository;

    public PermissionAuthorizeAttribute(IUserClaimsRepository userClaimsRepository,
        IUserRolesRepository userRolesRepository, IRoleClaimRepository roleClaimRepository)
    {
        _userClaimsRepository = userClaimsRepository;
        _userRolesRepository = userRolesRepository;
        _roleClaimRepository = roleClaimRepository;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var userIdStr = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userIdStr))
        {
            context.Result = new StatusCodeResult(401);
            return;
        }


        if (userName.ToLower().Equals("superadmin"))
            return;

        var userId = long.Parse(userIdStr);

        if (!CheckAuthorization(context, userId))
            context.Result = new StatusCodeResult(401);
    }

    private bool CheckAuthorization(AuthorizationFilterContext context, long userId)
    {
        var route = context.HttpContext.Request.Path.Value;
        var userPermissions = _userClaimsRepository.GetUserPermissions(userId).Select(claim => claim.Value);
        if (userPermissions.Contains(route, StringComparer.OrdinalIgnoreCase))
        {
            return true;
        }

        var userRoleIds = _userRolesRepository.GetUserRoleIds(userId);
        foreach (var userRoleId in userRoleIds)
        {
            if (_roleClaimRepository.GetRoleClaims(userRoleId).Select(claim => claim.Value)
                .Contains(route, StringComparer.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}