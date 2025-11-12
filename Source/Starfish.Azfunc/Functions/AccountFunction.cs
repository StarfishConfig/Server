using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc.Functions;

/// <summary>
/// Function to handle account-related operations.
/// </summary>
/// <param name="logger"></param>
/// <param name="service"></param>
public class AccountFunction(ILoggerFactory logger, IUserApplicationService service)
    : HttpFunctionBase<AccountFunction>(logger)
{
    /// <summary>
    /// Handles the create account request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [Function($"{nameof(AccountFunction)}-Create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account")] HttpRequest request, FunctionContext context)
    {
        return await ExecuteAsync(async () =>
        {
            var datamodel = await request.ReadFromJsonAsync<UserCreateDto>();
            if (datamodel == null)
            {
                throw new BadRequestException("Invalid request body.");
            }
            var result = await service.CreateAsync(datamodel, context.CancellationToken);
            return result;
        });
    }

    /// <summary>
    /// Handles the get profile request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [Function($"{nameof(AccountFunction)}-GetProfile")]
    public async Task<IActionResult> GetProfileAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/profile")] HttpRequest request, FunctionContext context)
    {
        return await ExecuteAsync(async () =>
        {
            var result = await service.GetProfileAsync(context.CancellationToken);
            return result;
        });
    }
}
