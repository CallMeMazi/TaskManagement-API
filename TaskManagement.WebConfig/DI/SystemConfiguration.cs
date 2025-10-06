using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskManagement.WebConfig.DI;
public static class SystemConfiguration
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, new[]
        {
            Assembly.Load(nameof(TaskManagement.Application)),
            Assembly.Load(nameof(TaskManagement.Infrastructure))
        });

        return services;
    }
}
