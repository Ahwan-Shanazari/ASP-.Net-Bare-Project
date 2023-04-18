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

    public PermissionAuthorizeAttribute(IUserClaimsRepository userClaimsRepository,IUserRolesRepository userRolesRepository)
    {
        _userClaimsRepository = userClaimsRepository;
        _userRolesRepository = userRolesRepository;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var userId = long.Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (string.IsNullOrEmpty(userName))
            context.Result = new StatusCodeResult(401);

        else if (!userName.ToLower().Equals("superadmin"))
            if(!CheckAuthorization(context,userId))
                context.Result = new StatusCodeResult(401);
    }

    private bool CheckAuthorization(AuthorizationFilterContext context,long userId)
    {
        var userClaims = _userClaimsRepository.GetUserPermissions(userId);
        var userRoleId = _userRolesRepository.GetUserRoleIds(userId);
        return true;
    }
}