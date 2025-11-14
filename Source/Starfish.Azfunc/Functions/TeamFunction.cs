using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Azfunc.Functions;

/// <summary>
/// Function to handle team-related operations.
/// </summary>
/// <param name="logger"></param>
/// <param name="service"></param>
public class TeamFunction(ILoggerFactory logger, ITeamApplicationService service)
	: HttpFunctionBase<TeamFunction>(logger)
{
	private const string ROUTE_PREFIX = "team";
	private const string FUNCTION_NAME = nameof(TeamFunction);

	/// <summary>
	/// Handles the find teams request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-Find")]
	public Task<IActionResult> FindAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = $"{ROUTE_PREFIX}/list")] HttpRequest request, FunctionContext context, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
	{
		return ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<TeamCriteriaDto>();
			var teams = await service.FindAsync(criteria, skip, take, context.CancellationToken);
			return teams;
		});
	}

	/// <summary>
	/// Handles the count teams request.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	[Function($"{FUNCTION_NAME}-Count")]
	public Task<IActionResult> CountAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = $"{ROUTE_PREFIX}/count")] HttpRequest request, FunctionContext context)
	{
		return ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<TeamCriteriaDto>();
			var count = await service.CountAsync(criteria, context.CancellationToken);
			return count;
		});
	}
}
