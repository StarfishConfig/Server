using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc.Functions;

/// <summary>
/// Function to handle token-related operations.
/// </summary>
/// <param name="logger"></param>
/// <param name="service"></param>
public class TokenFunction(ILoggerFactory logger, IAuthApplicationService service)
    : HttpFunctionBase<TokenFunction>(logger)
{
    /// <summary>
    /// Handles the token grant request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [Function("TokenFunction/Grant")]
    public async Task<IActionResult> GrantAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", "token/grant")] HttpRequest request, FunctionContext context)
    {
        return await ExecuteAsync(async () =>
        {
            var datamodel = await request.ReadFromJsonAsync<TokenGrantRequestDto>();
            if (datamodel == null)
            {
                throw new BadRequestException("Invalid request body.");
            }

            var result = await service.GrantAsync(datamodel, context.CancellationToken);
            return result;
        });
    }

    /// <summary>
    /// Handles the token refresh request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Function("TokenFunction/Refresh")]
    public async Task<IActionResult> RefreshAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", "token/refresh")] HttpRequest request, FunctionContext context, string token)
    {
        return await ExecuteAsync(async () =>
        {
            var result = await service.RefreshAsync(token, context.CancellationToken);
            return result;
        });
    }

    /// <summary>
    /// Handles the token introspection request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [Function("TokenFunction/Introspect")]
    public async Task<IActionResult> IntrospectAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", "token/introspect")] HttpRequest request, FunctionContext context)
    {
        return await ExecuteAsync(async () =>
        {
        });
    }

    /// <summary>
    /// Handles the token revocation request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Function("TokenFunction/Revoke")]
    public async Task<IActionResult> RevokeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", "token/revoke/{id}")] HttpRequest request, FunctionContext context, string id)
    {
        return await ExecuteAsync(async () =>
        {
            await service.RevokeAsync(id, context.CancellationToken);
            return new OkResult();
        });
    }
}
