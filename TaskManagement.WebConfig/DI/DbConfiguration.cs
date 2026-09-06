using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Infrastructure.IdGeneration;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.WebConfig.DI;
public static class DbConfiguration
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<EntityIdSaveChangesInterceptor>();
        services.AddApplicationDbContext(configuration);
        services.AddLogDbContext(configuration);

        return services;
    }

    private static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseSqlServer(configuration.GetConnectionString("ApplicationConnectionString"))
                .AddInterceptors(sp.GetRequiredService<EntityIdSaveChangesInterceptor>()),
            ServiceLifetime.Scoped
        );

        return services;
    }
    private static IServiceCollection AddLogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ApplicationLogConnectionString")),
            ServiceLifetime.Scoped
        );

        return services;
    }
}
