using CSharpAuth.Application.Services;
using CSharpAuth.Application.Services.Interfaces;

namespace CSharpAuth.Infrastructure.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();

        return services;
    }
}
