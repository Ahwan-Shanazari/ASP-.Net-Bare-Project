using System.Security.Claims;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Policies.PermissionPolicy;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserClaimsRepository _userClaimsRepository;
    private readonly IUserRolesRepository _userRolesRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRoleClaimRepository _roleClaimRepository;

    public PermissionHandler(IHttpContextAccessor httpContextAccessor, IRoleClaimRepository roleClaimRepository,
        IUserRolesRepository userRolesRepository, IUserClaimsRepository userClaimsRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _roleClaimRepository = roleClaimRepository;
        _userRolesRepository = userRolesRepository;
        _userClaimsRepository = userClaimsRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        
        var userIdStr = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userIdStr))
        {
            context.Fail();
            return Task.CompletedTask;
        }


        if (userName.ToLower().Equals("superadmin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }


        var userId = long.Parse(userIdStr);

        if (!CheckAuthorization(userId))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private bool CheckAuthorization(long userId)
    {
        var route = _httpContextAccessor.HttpContext.Request.Path.Value;
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