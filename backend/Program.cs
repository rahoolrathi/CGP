using System.Globalization;
using CgbSteps.Api.Data;
using CgbSteps.Api.Extensions;
using Serilog;

// Load settings from backend/.env (if the file exists) into environment variables.
DotNetEnv.Env.TraversePath().NoClobber().Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddLogging();
    builder.Services
        .AddDatabase(builder.Configuration)
        .AddRepositories()
        .AddAppServices()
        .AddApiInfrastructure(builder.Configuration);

    var app = builder.Build();

    if (args.Contains("migrate"))
    {
        await app.Services.GetRequiredService<DatabaseMigrator>().MigrateAsync(CancellationToken.None);
        return 0;
    }

    if (args.Contains("seed"))
    {
        await using var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DevDataSeeder>().SeedAsync(CancellationToken.None);
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
