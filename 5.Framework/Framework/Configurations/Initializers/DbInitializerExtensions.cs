using Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurations.Initializers;

public static class DbInitializerExtensions
{
    public async static Task<IApplicationBuilder> InitialDbWithSuperAdmin(this IApplicationBuilder app,
        IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetService<UserManager<IdentityUser<long>>>();
        var context = serviceProvider.GetService<DataContext>();
        if (!await userManager.Users.AnyAsync(user => user.UserName.Equals("SuperAdmin")))
        {
            var superAdmin = new IdentityUser<long>()
                { UserName = "SuperAdmin", Email = "SuperAdmin@gmail.com", LockoutEnabled = false };
            await userManager.CreateAsync(superAdmin, "1234a");
        }

        return app;
    }
}