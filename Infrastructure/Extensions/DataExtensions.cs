using CSharpAuth.Infrastructure.Repositories;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Infrastructure.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

        return services;
    }
}
