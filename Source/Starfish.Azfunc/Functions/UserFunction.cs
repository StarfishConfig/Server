using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Azfunc.Functions;

/// <summary>
/// Function to handle user-related operations.
/// </summary>
/// <param name="logger"></param>
/// <param name="service"></param>
public class UserFunction(ILoggerFactory logger, IUserApplicationService service)
	: HttpFunctionBase<UserFunction>(logger)
{
	private const string ROUTE_PREFIX = "user";
	private const string FUNCTION_NAME = nameof(UserFunction);

	/// <summary>
	/// Lists users based on specified criteria with pagination.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-Find")]
	public async Task<IActionResult> FindAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/list")] HttpRequest request, FunctionContext context, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<UserCriteriaDto>();
			var result = await service.FindAsync(criteria, skip, take, context.CancellationToken);
			return result;
		});
	}

	/// <summary>
	/// Counts users based on specified criteria.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-Count")]
	public async Task<IActionResult> CountAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/count")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<UserCriteriaDto>();
			var result = await service.CountAsync(criteria, context.CancellationToken);
			return result;
		});
	}

	/// <summary>
	/// Gets user detail by identifier.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-Detail")]
	public async Task<IActionResult> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(() => service.GetAsync(id, context.CancellationToken));
	}

	/// <summary>
	/// Resets the password of an existing user by its identifier.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-ResetPassword")]
	public async Task<IActionResult> ResetPasswordAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = $"{ROUTE_PREFIX}/{{id:long}}/password/reset")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(() => service.ResetPasswordAsync(id, context.CancellationToken));
	}

	/// <summary>
	/// Locks a user account by its identifier.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-Unlock")]
	public async Task<IActionResult> UnlockAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = $"{ROUTE_PREFIX}/{{id:long}}/unlock")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(() => service.UnlockAsync(id, context.CancellationToken));
	}
}