using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagement.Application.Features.User.Command.CreateUser;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Pipelines;
using TaskManagement.Application.Services.Application;
using TaskManagement.Domain.Interface.Services;
using TaskManagement.Domain.Services;

namespace TaskManagement.Application.Registration;

public static class ApplicationRegistration
{
    public static IServiceCollection RegisterAllApplicationLayerConfiguration(this IServiceCollection services)
    {
        services.RegisterAppServices()
            .RegisterDomainServices()
            .RegisterAutoMapper()
            .RegisterMediatR()
            .RegisterFluentValidation();

        return services;
    }
    public static void CompileAutoMapperConfiguration(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var configProvider = scope.ServiceProvider.GetRequiredService<AutoMapper.IConfigurationProvider>();
        configProvider.CompileMappings();
        configProvider.AssertConfigurationIsValid();
    }

    private static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthServiec, AuthService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskInfoService, TaskInfoService>();

        return services;
    }
    private static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IUserTokenDomainService, UserTokenDomainService>();
        services.AddScoped<IOrganizationDomainService, OrganizationDomainService>();
        services.AddScoped<IInvitationDomainService, InvitationDomainService>();
        services.AddScoped<IProjectDomainService, ProjectDomainService>();
        services.AddScoped<ITaskDomainService, TaskDomainService>();

        return services;
    }
    private static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, [Assembly.Load("TaskManagement.Application")]);

        return services;
    }
    private static IServiceCollection RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // Add Pipeline Behavior
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
    private static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

        return services;
    }
}
