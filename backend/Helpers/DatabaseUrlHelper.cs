using Npgsql;

namespace CgbSteps.Api.Helpers;

internal static class DatabaseUrlHelper
{
    /// <summary>
    /// Turns a URL like postgresql://user:pass@host/db (what Neon shows) into an Npgsql connection string.
    /// A value that is already in Key=Value form is returned as it is.
    /// </summary>
    public static string ToConnectionString(string value)
    {
        if (!value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
            && !value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        var uri = new Uri(value);
        var userInfo = uri.UserInfo.Split(':', 2);
        var isLocal = uri.IsLoopback;

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort || uri.Port <= 0 ? 5432 : uri.Port,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : null,
            SslMode = isLocal ? SslMode.Prefer : SslMode.VerifyFull,
        };

        return builder.ConnectionString;
    }
}
