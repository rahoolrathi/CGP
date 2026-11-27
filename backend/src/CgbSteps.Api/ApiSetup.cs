using System.Threading.RateLimiting;
using CgbSteps.Api.Endpoints;
using CgbSteps.Api.Infrastructure;
using CgbSteps.Application;
using CgbSteps.Application.Abstractions;
using CgbSteps.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using Serilog;

namespace CgbSteps.Api;

internal static class ApiSetup
{
    public const string AdminWebCorsPolicy = "admin-web";

    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
        builder.Host.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));

        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddSingleton<IApplicationInfo, AssemblyApplicationInfo>();

        services.AddProblemDetails(options => options.CustomizeProblemDetails = ProblemDetailsCustomizer.Apply);
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddCors(options => options.AddPolicy(AdminWebCorsPolicy, policy => policy
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

        return builder;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseMiddleware<SecurityHeadersMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseSerilogRequestLogging();
        app.UseCors(AdminWebCorsPolicy);
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi().AllowAnonymous();
            app.MapScalarApiReference().AllowAnonymous();
        }

        app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false })
            .AllowAnonymous()
            .DisableRateLimiting();
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(HealthCheckTags.Ready),
        }).AllowAnonymous().DisableRateLimiting();

        var v1 = app.MapGroup("/api/v1");
        v1.MapSystemEndpoints();

        return app;
    }
}
