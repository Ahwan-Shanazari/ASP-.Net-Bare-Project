using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Framework.Configurations;

public static class JwtConfigurationExtensions
{
    private static string FlattenException(Exception exception)
    {
        var stringBuilder = new StringBuilder();

        while (exception != null)
        {
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);

            exception = exception.InnerException;
        }

        return stringBuilder.ToString();
    }

    public static AuthenticationBuilder AddJwtServices(this IServiceCollection services)
    {
        string message = "";
        return services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtOptions:ValidIssuer"],
                    ValidAudience = configuration["JwtOptions:ValidAudience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"])),
                    TokenDecryptionKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:KeyX"])),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        ctx.Response.StatusCode = StatusCodes.Status302Found;
                        message += "From OnAuthenticationFailed:\n";
                        message += FlattenException(ctx.Exception);
                        return Task.CompletedTask;
                    },

                    OnChallenge = ctx =>
                    {
                        message += "From OnChallenge:\n";
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        ctx.Response.ContentType = "text/plain";
                        return ctx.Response.WriteAsync(message);
                    },

                    OnMessageReceived = ctx =>
                    {
                        message = "From OnMessageReceived:\n";
                        ctx.Request.Headers.TryGetValue("Authorization", out var BearerToken);
                        if (BearerToken.Count == 0)
                            BearerToken = "no Bearer token sent\n";
                        message += "Authorization Header sent: " + BearerToken + "\n";
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var claims = context.Principal.Claims;
                        var userName = claims.FirstOrDefault(claim =>
                            claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

                        var userManger = services.BuildServiceProvider().GetService<UserManager<IdentityUser<long>>>();
                        var user = await userManger.FindByNameAsync(userName.Value);

                        var stampClaim = claims.FirstOrDefault(claim => claim.Type == "SecurityStamp");
                        if (stampClaim is null || stampClaim.Value != user.SecurityStamp)
                            context.Fail("Security Stamp Is Expired");
                    }
                };
            });
    }
}