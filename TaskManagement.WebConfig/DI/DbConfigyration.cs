using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.WebConfig.DI;
public static class DbConfigyration
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string conecctionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(conecctionString)
        );

        return services;
    }

    public static IServiceCollection AddLogDbContext(this IServiceCollection services, string conecctionString)
    {
        services.AddDbContext<LogDbContext>(options =>
            options.UseSqlServer(conecctionString)
        );

        return services;
    }
}
