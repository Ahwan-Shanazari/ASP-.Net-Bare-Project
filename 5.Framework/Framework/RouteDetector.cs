using System.Reflection;
using Framework.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Framework;

public class RouteDetector : IRouteDetector
{
    public Dictionary<string, List<string>> GetAllRoutes(Assembly controllers)
    {
        Dictionary<string, List<string>> controllersAndActions = new();

        var controllerTypes = controllers.GetExportedTypes().Where(type =>
            !type.IsAbstract && !type.IsInterface && type.IsClass && type.IsAssignableTo(typeof(BaseController)));

        foreach (var controllerType in controllerTypes)
        {
            var actions = controllerType.GetMethods().Where(method =>
                    method.IsPublic && method.GetCustomAttribute<HttpMethodAttribute>() is not null)
                .Select(method => method.Name).ToList();
            controllersAndActions.Add(controllerType.Name.ToLower().Replace("controller", ""), actions);
        }

        return controllersAndActions;
    }
}