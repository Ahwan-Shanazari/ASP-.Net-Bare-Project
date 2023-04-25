using System.Reflection;
using Data.Contexts;
using Data.Repositories;
using Data.Repositories.Base;
using Framework;
using Framework.Configurations;
using Framework.Configurations.Initializers;
using Framework.CustomAttributes;
using Framework.Interfaces;
using Service;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//adding swagger with requirements
builder.Services.AddSwaggerWithJwtSupport();

builder.Services.AddSingleton<IRouteDetector, RouteDetector>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<PermissionAuthorizeAttribute>();
//ToDo: Inject the repositories dynamically using reflection and an assembly of data layer. look for their Interfaces and inject them with that. 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISuperAdminServices, SuperAdminServices>();
builder.Services.AddScoped<IUserClaimsRepository, UserClaimsRepository>();
builder.Services.AddScoped<IUserRolesRepository, UserRolesRepository>();
builder.Services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();

// Injecting The Context
builder.Services.AddDefaultIdentityForAContext<DataContext>(builder.Configuration,"PermissionAuthDb");

//adding authentication and jwt services
builder.Services.AddJwtServices();

var app = builder.Build();

await app.InitialDbWithSuperAdmin(app.Services.CreateScope().ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();