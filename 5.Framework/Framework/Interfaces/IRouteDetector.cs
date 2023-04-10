using System.Reflection;

namespace Framework.Interfaces;

public interface IRouteDetector
{
    Dictionary<string, string> GetAllRoutes(Assembly controllers);
}