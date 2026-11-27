namespace CgbSteps.Api.Infrastructure;

internal sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    private const string ApiContentSecurityPolicy = "default-src 'none'; frame-ancestors 'none'";

    public Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;
        headers.XContentTypeOptions = "nosniff";
        headers.XFrameOptions = "DENY";
        headers["Referrer-Policy"] = "no-referrer";
        headers["Cross-Origin-Resource-Policy"] = "same-site";

        if (!context.Request.Path.StartsWithSegments("/scalar"))
        {
            headers.ContentSecurityPolicy = ApiContentSecurityPolicy;
        }

        return next(context);
    }
}
