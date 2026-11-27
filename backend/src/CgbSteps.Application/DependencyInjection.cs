using CgbSteps.Application.Status;
using Microsoft.Extensions.DependencyInjection;

namespace CgbSteps.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<GetSystemInfoHandler>();
        return services;
    }
}
