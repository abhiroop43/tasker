using Microsoft.AspNetCore.Diagnostics;

namespace Tasker.Api.Handlers;

public class CustomExceptionHandler(
    ILogger<CustomExceptionHandler> logger,
    IWebHostEnvironment environment
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        logger.LogError(
            exception,
            "An exception occurred while processing {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path
        );

        string detail,
            title;
        int statusCode;

        switch (exception)
        {
            case ValidationException:
            case BadRequestException:
                detail = exception.Message;
                title = exception.GetType().Name;
                statusCode = StatusCodes.Status400BadRequest;
                break;

            case NotFoundException:
                detail = exception.Message;
                title = exception.GetType().Name;
                statusCode = StatusCodes.Status404NotFound;
                break;

            default:
                detail = environment.IsDevelopment()
                    ? exception.Message
                    : "An error has occurred. Please contact the Administrator";
                title = environment.IsDevelopment()
                    ? exception.GetType().Name
                    : "Internal Server Error";
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = httpContext.Request.Path,
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
            problemDetails.Extensions["errors"] = validationException
                .Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(x => x.ErrorMessage).ToArray()
                );
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
