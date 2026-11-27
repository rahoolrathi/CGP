using System.Globalization;
using CgbSteps.Api.Data;
using CgbSteps.Api.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddLogging();
    builder.Services
        .AddDatabase(builder.Configuration)
        .AddAppServices()
        .AddApiInfrastructure(builder.Configuration);

    var app = builder.Build();

    if (args.Contains("migrate"))
    {
        await app.Services.GetRequiredService<DatabaseMigrator>().MigrateAsync(CancellationToken.None);
        return 0;
    }

    app.UseApiPipeline();
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
