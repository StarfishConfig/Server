using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc;

internal class ExceptionHandlingMiddleware(ILoggerFactory logger) : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger.CreateLogger<ExceptionHandlingMiddleware>();

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Message}", exception.Message);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(FunctionContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            status = statusCode,
            message = GetMessage(exception),
            details = GetErrors(exception)
        };

        var httpContext = context.GetHttpContext()!;

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        return httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetMessage(Exception exception)
    {
        return exception switch
        {
            AggregateException ex => ex.InnerException == null ? ex.Message : GetMessage(ex.InnerException),
            TargetInvocationException ex => ex.InnerException == null ? ex.Message : GetMessage(ex.InnerException),
            _ => exception.Message
        };
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            HttpStatusException ex => (int)ex.StatusCode,
            AuthenticationException => StatusCodes.Status401Unauthorized,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            ValidationException => StatusCodes.Status400BadRequest,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            AggregateException ex => GetStatusCode(ex.InnerException),
            TargetInvocationException ex => ex.InnerException == null ? StatusCodes.Status500InternalServerError : GetStatusCode(ex.InnerException),
            _ => (int)(exception.GetType().GetCustomAttribute<HttpStatusCodeAttribute>()?.StatusCode ?? HttpStatusCode.InternalServerError)
        };
    }

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        if (exception is AggregateException)
        {
            return GetErrors(exception.InnerException);
        }

        if (exception is not ValidationException ex)
        {
            return null;
        }

        return ex.Errors
                 .GroupBy(t => t.PropertyName)
                 .ToDictionary(t => t.Key, t => t.Select(x => x.ErrorMessage).ToArray());
    }
}