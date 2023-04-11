using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.CustomAttributes;

public class PermissionAuthorizeAttribute :Attribute,IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(userName))
        {
            context.Result = new StatusCodeResult(401);
            return;
        }
            
        if (!userName.ToLower().Equals("superadmin"))
            context.Result = new StatusCodeResult(401);

    }
}