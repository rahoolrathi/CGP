using CgbSteps.Api.Extensions;
using CgbSteps.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Api.Data;

public sealed partial class DatabaseMigrator(IConfiguration configuration, ILogger<DatabaseMigrator> logger)
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetRequiredConnectionString(AppConstants.ConnectionStrings.Migrations);

        var options = new DbContextOptionsBuilder<AppDbContext>();
        AppDbContext.Configure(options, connectionString);

        await using var context = new AppDbContext(options.Options);
        var pending = (await context.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();

        LogApplying(logger, pending.Count);
        await context.Database.MigrateAsync(cancellationToken);
        LogUpToDate(logger);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Applying {Count} pending migration(s)")]
    private static partial void LogApplying(ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Database is up to date")]
    private static partial void LogUpToDate(ILogger logger);
}
