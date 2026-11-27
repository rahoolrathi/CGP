namespace CgbSteps.Api.Helpers;

public static class AppConstants
{
    public const string AdminWebCorsPolicy = "admin-web";
    public const string ReadyHealthTag = "ready";

    public static class DatabaseKeys
    {
        public const string App = "DATABASE_URL";
        public const string Migrations = "MIGRATIONS_DATABASE_URL";
    }

    public static class ErrorCodes
    {
        public const string BadRequest = "bad_request";
        public const string Unauthorized = "unauthorized";
        public const string Forbidden = "forbidden";
        public const string NotFound = "not_found";
        public const string MethodNotAllowed = "method_not_allowed";
        public const string RateLimited = "rate_limited";
        public const string ServerError = "server_error";
    }
}
