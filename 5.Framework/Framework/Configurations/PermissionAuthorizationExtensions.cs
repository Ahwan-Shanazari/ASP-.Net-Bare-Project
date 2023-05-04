using Framework.Interfaces;
using Framework.Policies.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurations;

public static class PermissionAuthorizationExtensions
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IRouteDetector, RouteDetector>();
        
        services.AddScoped<IAuthorizationHandler,PermissionHandler>();
        
        //adding authorization policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("PermissionPolicy", policy =>
            {
                policy.Requirements.Add(new PermissionRequirement());
            });
        });
        
        return services;
    }
}