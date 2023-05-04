using System.Security.Claims;
using Data.Contexts;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.CustomAttributes;

//ToDo: must also implement super admin check from json or custom configuration class 
public class PermissionAuthorizeAttribute : AuthorizeAttribute,IAuthorizationFilter
{
    public PermissionAuthorizeAttribute()
    {
        Policy = "PermissionPolicy";
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!Policy?.Equals("PermissionPolicy") ?? true)
        {
            throw new Exception("Invalid Usage Of PermissionPolicy");
        }
    }
}