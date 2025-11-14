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
public partial class TeamFunction(ILoggerFactory logger, ITeamApplicationService service)
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
	public Task<IActionResult> FindAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/list")] HttpRequest request, FunctionContext context, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
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
	public Task<IActionResult> CountAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/count")] HttpRequest request, FunctionContext context)
	{
		return ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<TeamCriteriaDto>();
			var count = await service.CountAsync(criteria, context.CancellationToken);
			return count;
		});
	}

	[Function($"{FUNCTION_NAME}-Detail")]
	public Task<IActionResult> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return ExecuteAsync(async () =>
		{
			var team = await service.GetAsync(id, context.CancellationToken);
			return team;
		});
	}

	[Function($"{FUNCTION_NAME}-Create")]
	public Task<IActionResult> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ROUTE_PREFIX)] HttpRequest request, FunctionContext context)
	{
		return ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<TeamCreateDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			var result = await service.CreateAsync(datamodel, context.CancellationToken);
			return new CreatedResultDto<long>(result);
		});
	}

	[Function($"{FUNCTION_NAME}-Update")]
	public Task<IActionResult> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<TeamUpdateDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.UpdateAsync(id, datamodel, context.CancellationToken);
		});
	}

	[Function($"{FUNCTION_NAME}-Delete")]
	public Task<IActionResult> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return ExecuteAsync(async () =>
		{
			await service.DeleteAsync(id, context.CancellationToken);
		});
	}

	[Function($"{FUNCTION_NAME}-Transfer")]
	public Task<IActionResult> TransferAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/{{id:long}}/transfer")] HttpRequest request, FunctionContext context, long id)
	{
		return ExecuteAsync(async () =>
		{
			var datamodel = await request.ReadFromJsonAsync<TeamTransferDto>();
			if (datamodel == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.TransferAsync(id, datamodel, context.CancellationToken);
		});
	}
}
