using CgbSteps.Api.Helpers;

namespace CgbSteps.Api.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>Reads a database URL (for example DATABASE_URL from backend/.env) and returns a connection string.</summary>
    public static string GetDatabaseConnectionString(this IConfiguration configuration, string key) =>
        configuration[key] is { Length: > 0 } value
            ? DatabaseUrlHelper.ToConnectionString(value)
            : throw new InvalidOperationException($"{key} is not set. Add it to backend/.env (see backend/.env.example).");
}
