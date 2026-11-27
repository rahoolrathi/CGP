using CgbSteps.Application.Status;

namespace CgbSteps.Api.Endpoints;

internal static class SystemEndpoints
{
    public static IEndpointRouteBuilder MapSystemEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/system").WithTags("System");

        group.MapGet("/info", (GetSystemInfoHandler handler) => TypedResults.Ok(handler.Handle()))
            .WithName("GetSystemInfo")
            .AllowAnonymous();

        return routes;
    }
}
