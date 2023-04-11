using System.Reflection;

namespace Framework.Interfaces;

public interface IRouteDetector
{
    Dictionary<string, List<string>> GetAllRoutes(Assembly controllers);
}