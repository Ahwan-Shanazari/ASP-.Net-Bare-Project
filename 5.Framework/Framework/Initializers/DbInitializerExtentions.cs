using Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Initializers;

public static class DbInitializerExtentions
{
    public async static Task<IApplicationBuilder> InitialDbWithSuperAdmin(this IApplicationBuilder app,
        IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser<long>>>();
            var context = scope.ServiceProvider.GetService<DataContext>();
            if (!await userManager.Users.AnyAsync())
            {
                var superAdmin = new IdentityUser<long>() { UserName = "SuperAdmin", Email = "SuperAdmin@gmail.com",LockoutEnabled = false};
                await userManager.CreateAsync(superAdmin, "1234a");
            }
        }

        return app;
    }
}