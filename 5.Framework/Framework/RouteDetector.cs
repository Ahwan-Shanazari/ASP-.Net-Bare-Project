using System.Reflection;
using Framework.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Framework;

public class RouteDetector : IRouteDetector
{
    public Dictionary<string, string> GetAllRoutes(Assembly controllers)
    {
        Dictionary<string, string> controllersAndActions = new();

        var controllerTypes = controllers.GetExportedTypes().Where(type =>
            !type.IsAbstract && !type.IsInterface && type.IsClass && type.IsAssignableTo(typeof(BaseController)));

        foreach (var controllerType in controllerTypes)
        {
            foreach (var action in controllerType.GetMethods()
                         .Where(method => method.IsPublic && method.GetCustomAttribute<HttpMethodAttribute>() is not null))
            {
                controllersAndActions.Add(controllerType.Name,action.Name);
            }
        }

        return controllersAndActions;
    }
}