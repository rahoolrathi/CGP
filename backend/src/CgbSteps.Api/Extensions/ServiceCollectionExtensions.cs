using System.Threading.RateLimiting;
using CgbSteps.Api.Data;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Interfaces;
using CgbSteps.Api.Middleware;
using CgbSteps.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace CgbSteps.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
        builder.Host.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));
        return builder;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredConnectionString(AppConstants.ConnectionStrings.App);

        services.AddDbContext<AppDbContext>(options => AppDbContext.Configure(options, connectionString));
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("database", tags: [AppConstants.ReadyHealthTag]);
        services.AddTransient<DatabaseMigrator>();

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ISystemService, SystemService>();
        return services;
    }

    public static IServiceCollection AddApiInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddProblemDetails(options => options.CustomizeProblemDetails = ProblemDetailsHelper.AddCodeAndTraceId);
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddCors(options => options.AddPolicy(AppConstants.AdminWebCorsPolicy, policy => policy
            .WithOrigins(configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
            .WithHeaders("Content-Type", "Authorization", "X-CSRF-Token", "Idempotency-Key")
            .AllowCredentials()));

        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters.ValidIssuer = configuration["Auth:Issuer"];
            options.TokenValidationParameters.ValidAudience = configuration["Auth:Audience"];
        });

        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

        services.AddRateLimiter(options =>
        {
            var permitLimit = configuration.GetValue("RateLimiting:PermitsPerMinute", 120);
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = TimeSpan.FromMinutes(1),
                    }));
        });

        return services;
    }
}
