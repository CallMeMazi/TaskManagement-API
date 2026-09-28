using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskManagement.Application.Interfaces.Repositories.Log;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Settings;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;
using TaskManagement.Infrastructure.Persistence.Interceptors;
using TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
using TaskManagement.Infrastructure.Repositories.LogRepositories;
using TaskManagement.Infrastructure.Services;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Infrastructure.Registration;

public static class InfrastructureRegistration
{
    public static IServiceCollection RegisterAllInfrastructureLayerConfiguration(this IServiceCollection services
        , IConfiguration configuration)
    {
        services.RegisterAppDbContext(configuration)
            .RegisterLogDbContext(configuration)
            .RegisterAppRepositories()
            .RegisterLogRepositpries()
            .RegisterUnitOfWorks()
            .RegisterHelperServices()
            .RegisterAppSettingConfig(configuration);

        return services;
    }

    private static IServiceCollection RegisterAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NormalizeStringInterceptor>();
        services.AddSingleton<EntityIdSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseSqlServer(configuration.GetConnectionString("ApplicationConnectionString"))
                .AddInterceptors(
                    sp.GetRequiredService<NormalizeStringInterceptor>(),
                    sp.GetRequiredService<EntityIdSaveChangesInterceptor>()),
            ServiceLifetime.Scoped
        );

        return services;
    }
    private static IServiceCollection RegisterLogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ApplicationLogConnectionString")),
            ServiceLifetime.Scoped
        );

        return services;
    }
    private static IServiceCollection RegisterAppRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IOrganizationMemberShipRepository, OrganizationMemberShipRepository>();
        services.AddScoped<IOrganizationInvitationRepository, OrganizationInvitationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectMemberShipRepository, ProjectMemberShipRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
        services.AddScoped<ITaskInfoRepository, TaskInfoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    private static IServiceCollection RegisterLogRepositpries(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILogBaseRepository<>), typeof(LogBaseRepository<>));
        services.AddScoped<IEntityLogRepository, EntityLogRepository>();
        services.AddScoped<IEntityRelationLogRepository, EntityRelationLogRepository>();

        return services;
    }
    private static IServiceCollection RegisterUnitOfWorks(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILogUnitOfWork, LogUnitOfWork>();

        return services;
    }
    private static IServiceCollection RegisterHelperServices(this IServiceCollection services)
    {
        services.AddSingleton<ICommonService, CommonService>();
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IIdGeneratorService, IdGeneratorService>();

        return services;
    }
    private static IServiceCollection RegisterAppSettingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

        return services;
    }
}
