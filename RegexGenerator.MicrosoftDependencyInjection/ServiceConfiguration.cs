using Microsoft.Extensions.DependencyInjection;

namespace Rangex.MicrosoftDependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection AddRangex(this IServiceCollection services)
    {
        return services;
    }
}