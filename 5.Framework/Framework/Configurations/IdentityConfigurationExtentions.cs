using Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurations;

public static class IdentityConfigurationExtensions
{
    public static IServiceCollection AddDefaultIdentityForAContext<TContext>(this IServiceCollection services,
        IConfiguration configuration,string connectionsStringName)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(optionsBuilder =>
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(connectionsStringName)));
        services.AddIdentity<IdentityUser<long>, IdentityRole<long>>().AddEntityFrameworkStores<DataContext>();
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        return services;
    }

    public static IServiceCollection AddIdentityForAContext<TContext, TUser, TRole>(this IServiceCollection services,
        IConfiguration configuration,string connectionsStringName)
        where TContext : DbContext
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        services.AddDbContext<TContext>(optionsBuilder =>
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(connectionsStringName)));
        services.AddIdentity<TUser, TRole>().AddEntityFrameworkStores<DataContext>();
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        return services;
    }
}