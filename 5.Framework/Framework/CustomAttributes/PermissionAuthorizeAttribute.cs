using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.CustomAttributes;
//ToDo: must also implement super admin check from json or custom configuration class 
public class PermissionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(userName))
            context.Result = new StatusCodeResult(401);

        else if (!userName.ToLower().Equals("superadmin"))
            context.Result = new StatusCodeResult(401);
    }
}