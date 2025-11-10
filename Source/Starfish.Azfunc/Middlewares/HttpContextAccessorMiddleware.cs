using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Nerosoft.Starfish.Azfunc;

/// <summary>
/// This middleware set the <see cref="HttpContext"/> to the <see cref="IHttpContextAccessor"/>.
/// </summary>
/// <remarks>
/// The HttpConte is not available in the Azure Functions Worker, so it should be set manually while processing the request.
/// </remarks>
internal class HttpContextAccessorMiddleware : IFunctionsWorkerMiddleware
{
    public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();
        context.InstanceServices.GetService<IHttpContextAccessor>().HttpContext = httpContext;
        return next(context);
    }
}