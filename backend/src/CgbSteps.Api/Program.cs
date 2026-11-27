using System.Globalization;
using CgbSteps.Api;
using CgbSteps.Infrastructure.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console(formatProvider: CultureInfo.InvariantCulture).CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddApi();
    var app = builder.Build();

    if (args.Contains("migrate"))
    {
        await app.Services.GetRequiredService<DatabaseMigrator>().MigrateAsync(CancellationToken.None);
        return 0;
    }

    app.UseApi();
    await app.RunAsync();
    return 0;
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program;
