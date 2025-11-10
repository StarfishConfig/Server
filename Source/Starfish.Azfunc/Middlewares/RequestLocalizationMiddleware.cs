using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Nerosoft.Starfish.Azfunc;

/// <summary>
/// This middleware sets the current thread's culture based on the incoming HTTP request.
/// </summary>
internal class RequestLocalizationMiddleware : IFunctionsWorkerMiddleware
{
    /// <summary>
    /// Invokes the middleware to set the thread's culture.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext != null)
        {
            var culture = GetCulture(httpContext);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        }

        {
        }

        return next(context);
    }

    /// <summary>
    /// Gets the culture from the HTTP context.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static string GetCulture(HttpContext context)
    {
        if (context == null)
        {
            return CultureInfo.CurrentCulture.Name;
        }

        if (context.User.FindFirstValue(ClaimTypes.Locality) is { } claimValue)
        {
            return claimValue;
        }

        var requestCulture = context.Request.Headers.AcceptLanguage;

        if (requestCulture.Count <= 0)
        {
            return CultureInfo.CurrentCulture.Name;
        }

        var languages = requestCulture[0]!.Split(',');
        return languages.Length > 0 ? languages[0] : CultureInfo.CurrentCulture.Name;
    }
}