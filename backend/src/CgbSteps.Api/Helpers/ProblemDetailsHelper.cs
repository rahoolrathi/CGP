using System.Diagnostics;
using CgbSteps.Api.Utils;

namespace CgbSteps.Api.Helpers;

internal static class ProblemDetailsHelper
{
    public static void AddCodeAndTraceId(ProblemDetailsContext context)
    {
        var problem = context.ProblemDetails;
        problem.Extensions.TryAdd("code", ErrorCodeFor(problem.Status));
        problem.Extensions.TryAdd("traceId", Activity.Current?.Id ?? context.HttpContext.TraceIdentifier);
    }

    public static string ErrorCodeFor(int? status) => status switch
    {
        StatusCodes.Status400BadRequest => AppConstants.ErrorCodes.BadRequest,
        StatusCodes.Status401Unauthorized => AppConstants.ErrorCodes.Unauthorized,
        StatusCodes.Status403Forbidden => AppConstants.ErrorCodes.Forbidden,
        StatusCodes.Status404NotFound => AppConstants.ErrorCodes.NotFound,
        StatusCodes.Status405MethodNotAllowed => AppConstants.ErrorCodes.MethodNotAllowed,
        StatusCodes.Status429TooManyRequests => AppConstants.ErrorCodes.RateLimited,
        _ => AppConstants.ErrorCodes.ServerError,
    };
}
