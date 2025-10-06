using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.UnitOfWork;

namespace TaskManagement.WebConfig.DI;
public static class CustomConfiguration
{
    public static IServiceCollection AddUnitOfWorkConfig(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    public static IServiceCollection AddRepositoriesConfig(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskInfoRepository, TaskInfoRepository>();

        return services;
    }
    public static IServiceCollection AddServicesConfig(this IServiceCollection services)
    {
        return services;
    }
}
