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
	private const string ROUTE_PREFIX = "account";
	private const string FUNCTION_NAME = nameof(AccountFunction);

	/// <summary>
	/// Handles the create account request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{nameof(AccountFunction)}-Create")]
	[AllowAnonymous]
	public async Task<IActionResult> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ROUTE_PREFIX)] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<UserCreateDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			var result = await service.CreateAsync(datamodel, context.CancellationToken);
			return new CreatedResultDto<long>(result);
		});
	}

	/// <summary>
	/// Handles the get profile request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	[Function($"{nameof(AccountFunction)}-GetProfile")]
	public async Task<IActionResult> GetProfileAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/profile")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var result = await service.GetProfileAsync(context.CancellationToken);
			return result;
		});
	}

	/// <summary>
	/// Handles the change password request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{FUNCTION_NAME}-ChangePassword")]
	public async Task<IActionResult> ChangePasswordAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/password/change")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<UserPasswordChangeDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.ChangePasswordAsync(datamodel, context.CancellationToken);
		});
	}

	/// <summary>
	/// Handles the reset password request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{FUNCTION_NAME}-ResetPassword")]
	public async Task<IActionResult> ResetPasswordAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/password/reset")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<UserPasswordResetDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.ResetPasswordAsync(datamodel, context.CancellationToken);
		});
	}

	/// <summary>
	/// Handles the update phone request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{FUNCTION_NAME}-UpdatePhone")]
	public async Task<IActionResult> UpdatePhoneAsync([HttpTrigger(AuthorizationLevel.Anonymous, "patch", "post", Route = $"{ROUTE_PREFIX}/phone")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<UserPhoneUpdateDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.UpdatePhoneAsync(datamodel.Phone, context.CancellationToken);
		});
	}

	/// <summary>
	/// Handles the update email request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{FUNCTION_NAME}-UpdateEmail")]
	public async Task<IActionResult> UpdateEmailAsync([HttpTrigger(AuthorizationLevel.Anonymous, "patch", "post", Route = $"{ROUTE_PREFIX}/email")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<UserEmailUpdateDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.UpdateEmailAsync(datamodel.Email, context.CancellationToken);
		});
	}
}
