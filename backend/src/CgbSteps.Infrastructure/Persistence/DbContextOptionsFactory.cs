using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Infrastructure.Persistence;

internal static class DbContextOptionsFactory
{
    public static void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options
            .UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure())
            .UseSnakeCaseNamingConvention();
    }
}
