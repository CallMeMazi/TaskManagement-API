using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskManagement.WebConfig.DI;
public static class SystemConfiguration
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, new[]
        {
            Assembly.Load("TaskManagement.Application"),
            Assembly.Load("TaskManagement.Infrastructure")
        });

        return services;
    }
}
