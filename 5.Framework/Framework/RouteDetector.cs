using System.Reflection;
using Framework.CustomAttributes;
using Framework.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Framework;

public class RouteDetector : IRouteDetector
{
    //ToDo: Fix The Return Type
    public Dictionary<string, List<string>> GetAllRoutes(Assembly controllers)
    {
        Dictionary<string, List<string>> controllersAndActions = new();

        var controllerTypes = controllers.GetExportedTypes().Where(type =>
            !type.IsAbstract && !type.IsInterface && type.IsClass && type.IsAssignableTo(typeof(BaseController)));
        foreach (var controllerType in controllerTypes)
        {
            List<string> actions;
            if (controllerType.GetCustomAttribute<PermissionAuthorizeAttribute>() is not null)
            {
                actions = controllerType.GetMethods().Where(method =>
                        method.IsPublic && method.GetCustomAttribute<HttpMethodAttribute>() is not null)
                    .Select(method => method.Name).ToList();
            }
            else
            {
                actions = controllerType.GetMethods().Where(method =>
                        method.IsPublic && method.GetCustomAttribute<PermissionAuthorizeAttribute>() is not null)
                    .Select(method => method.Name).ToList();
            }

            if (actions.Count > 0)
                controllersAndActions.Add(controllerType.Name.ToLower().Replace("controller", ""), actions);
        }

        return controllersAndActions;
    }
}