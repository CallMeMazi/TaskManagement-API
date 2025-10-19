using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskManagement.WebConfig.DI;
public static class SystemConfiguration
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { },
        [
            Assembly.Load("TaskManagement.Application"),
            Assembly.Load("TaskManagement.Infrastructure")
        ]);

        return services;
    }

    public static void CompileMappings(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var configProvider = scope.ServiceProvider.GetRequiredService<AutoMapper.IConfigurationProvider>();
        configProvider.CompileMappings();
        configProvider.AssertConfigurationIsValid();
    }
}
