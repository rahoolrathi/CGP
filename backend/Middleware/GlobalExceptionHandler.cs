using CgbSteps.Api.Exceptions;
using CgbSteps.Api.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CgbSteps.Api.Middleware;

internal sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            return true;
        }

        var (status, code) = exception switch
        {
            AppException app => (app.StatusCode, app.Code),
            DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } } =>
                (StatusCodes.Status409Conflict, AppConstants.ErrorCodes.Conflict),
            _ => (StatusCodes.Status500InternalServerError, AppConstants.ErrorCodes.ServerError),
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            LogUnhandled(logger, exception, httpContext.Request.Method, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = status;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = { Status = status, Extensions = { ["code"] = code } },
        });
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled exception for {Method} {Path}")]
    private static partial void LogUnhandled(ILogger logger, Exception exception, string method, string path);
}
