using CgbSteps.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CgbSteps.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringNames.App)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStringNames.App}' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            DbContextOptionsFactory.Configure(options, connectionString));

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("database", tags: [HealthCheckTags.Ready]);
        services.AddTransient<DatabaseMigrator>();

        return services;
    }
}
