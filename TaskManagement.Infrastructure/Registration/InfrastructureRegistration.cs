using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Settings;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;
using TaskManagement.Infrastructure.Persistence.Interceptors;
using TaskManagement.Infrastructure.Queues;
using TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
using TaskManagement.Infrastructure.Services;
using TaskManagement.Infrastructure.UnitOfWorks;
using TaskManagement.Infrastructure.Worker;

namespace TaskManagement.Infrastructure.Registration;

public static class InfrastructureRegistration
{
    // Support in .net 10
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterAllInfrastructureLayerConfiguration(IConfiguration configuration)
            => services.RegisterAppDbContext(configuration)
            .RegisterLogDbContext(configuration)
            .RegisterAppRepositories()
            .RegisterUnitOfWorks()
            .RegisterHelperServices()
            .RegisterAppSettingConfig(configuration)
            .RegisterQueues()
            .RegisterWorkers();
    }

    private static IServiceCollection RegisterAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NormalizeStringInterceptor>()
            .AddSingleton<EntityIdSaveChangesInterceptor>();

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
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserTokenRepository, UserTokenRepository>()
            .AddScoped<IOrganizationRepository, OrganizationRepository>()
            .AddScoped<IOrganizationMemberShipRepository, OrganizationMemberShipRepository>()
            .AddScoped<IOrganizationInvitationRepository, OrganizationInvitationRepository>()
            .AddScoped<IProjectRepository, ProjectRepository>()
            .AddScoped<IProjectMemberShipRepository, ProjectMemberShipRepository>()
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>()
            .AddScoped<ITaskInfoRepository, TaskInfoRepository>()
            .AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    private static IServiceCollection RegisterUnitOfWorks(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    private static IServiceCollection RegisterHelperServices(this IServiceCollection services)
    {
        services.AddSingleton<ICommonService, CommonService>()
            .AddSingleton<IPasswordService, PasswordService>()
            .AddSingleton<IJwtService, JwtService>()
            .AddScoped<IAuditLogService, AuditLogService>()
            .AddSingleton<IIdGeneratorService, IdGeneratorService>()
            .AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
    private static IServiceCollection RegisterAppSettingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

        return services;
    }
    private static IServiceCollection RegisterQueues(this IServiceCollection services)
    {
        services.AddSingleton<IAuditQueue, AuditQueue>();

        return services;    
    }
    private static IServiceCollection RegisterWorkers(this IServiceCollection services)
    {
        services.AddHostedService<AuditLogWorker>();

        return services;
    }
}
