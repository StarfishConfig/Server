using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Azfunc.Functions;

/// <summary>
/// Project Function
/// </summary>
/// <param name="logger"></param>
/// <param name="service"></param>
public class ProjectFunction(ILoggerFactory logger, IProjectApplicationService service)
	: HttpFunctionBase<ProjectFunction>(logger)
{
	private const string ROUTE_PREFIX = "project";
	private const string FUNCTION_NAME = nameof(ProjectFunction);

	[Function($"{FUNCTION_NAME}-Find")]
	public async Task<IActionResult> FindAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/list")] HttpRequest request, FunctionContext context, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<ProjectCriteriaDto>();
			var result = await service.FindAsync(criteria, skip, take, context.CancellationToken);
			return result;
		});
	}

	[Function($"{FUNCTION_NAME}-Cound")]
	public async Task<IActionResult> CountAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/count")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<ProjectCriteriaDto>();
			var result = await service.CountAsync(criteria, context.CancellationToken);
			return result;
		});
	}

	[Function($"{FUNCTION_NAME}-Detail")]
	public async Task<IActionResult> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			var result = await service.GetAsync(id, context.CancellationToken);
			return result;
		});
	}

	[Function($"{FUNCTION_NAME}-Create")]
	public async Task<IActionResult> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var data = await request.ReadFromJsonAsync<ProjectCreateDto>();
			if (data == null)
			{
				throw new BadRequestException("Project data cannot be null.");
			}

			var result = await service.CreateAsync(data, context.CancellationToken);
			return new CreatedResultDto<long>(result);
		});
	}

	[Function($"{FUNCTION_NAME}-Update")]
	public async Task<IActionResult> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			var data = await request.ReadFromJsonAsync<ProjectUpdateDto>();
			if (data == null)
			{
				throw new BadRequestException("Project data cannot be null.");
			}

			await service.UpdateAsync(id, data, context.CancellationToken);
		});
	}

	[Function($"{FUNCTION_NAME}-Delete")]
	public async Task<IActionResult> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = $"{ROUTE_PREFIX}/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(() => service.DeleteAsync(id, context.CancellationToken));
	}
}