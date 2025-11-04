using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Settings;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.UnitOfWork;

namespace TaskManagement.WebConfig.DI;
public static class CustomConfiguration
{
    public static IServiceCollection AddAppServicesConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUnitOfWorkConfig();
        services.AddRepositoriesConfig();
        services.AddServicesConfig();
        services.AddAppSettingConfig(configuration);

        return services;
    }

    private static IServiceCollection AddUnitOfWorkConfig(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    private static IServiceCollection AddRepositoriesConfig(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskInfoRepository, TaskInfoRepository>();

        return services;
    }
    private static IServiceCollection AddServicesConfig(this IServiceCollection services)
    {

        return services;
    }
    private static IServiceCollection AddAppSettingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<AppSettings>>().Value
        );

        return services;
    }
}
