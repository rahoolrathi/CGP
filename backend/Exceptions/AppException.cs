namespace CgbSteps.Api.Exceptions;

/// <summary>An expected error, returned to the client as a problem response with a status and a code.</summary>
public sealed class AppException : Exception
{
    public AppException()
        : this(StatusCodes.Status400BadRequest, "bad_request")
    {
    }

    public AppException(string message)
        : base(message)
    {
        StatusCode = StatusCodes.Status400BadRequest;
        Code = "bad_request";
    }

    public AppException(string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = StatusCodes.Status400BadRequest;
        Code = "bad_request";
    }

    public AppException(int statusCode, string code)
        : base(code)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public int StatusCode { get; }

    public string Code { get; }

    public static AppException BadRequest(string code) => new(StatusCodes.Status400BadRequest, code);

    public static AppException NotFound(string code) => new(StatusCodes.Status404NotFound, code);

    public static AppException Conflict(string code) => new(StatusCodes.Status409Conflict, code);
}
