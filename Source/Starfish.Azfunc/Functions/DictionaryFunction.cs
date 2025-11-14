using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Azfunc.Functions;

public class DictionaryFunction(ILoggerFactory logger, IDictionaryApplicationService service)
	: HttpFunctionBase<DictionaryFunction>(logger)
{
	private const string ROUTE_PREFIX = "dictionary";
	private const string FUNCTION_NAME = nameof(DictionaryFunction);

	[Function($"{FUNCTION_NAME}-FindRoot")]
	public async Task<IActionResult> FindRootAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/root/list")] HttpRequest request, FunctionContext context, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<DictionaryRootCriteriaDto>();
			var result = await service.FindRootAsync(criteria, skip, take, context.CancellationToken);
			return result;
		});
	}

	[Function($"{FUNCTION_NAME}-CountRoot")]
	public async Task<IActionResult> CountRootAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/root/count")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<DictionaryRootCriteriaDto>();
			var result = await service.CountRootAsync(criteria, context.CancellationToken);
			return result;
		});
	}

	[Function($"{FUNCTION_NAME}-Lookup")]
	public async Task<IActionResult> LookupAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/lookup")] HttpRequest request, FunctionContext context, bool? isValid = null)
	{
		return await ExecuteAsync(async () =>
		{
			var codes = await request.ReadFromJsonAsync<string[]>();
			if (codes?.Any() != true)
			{
				throw new BadRequestException("Codes cannot be null or empty.");
			}

			var result = await service.LookupAsync(codes, isValid, context.CancellationToken);
			return result;
		});
	}

	[Function($"{FUNCTION_NAME}-CreateRoot")]
	public async Task<IActionResult> CreateRootAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/root")] HttpRequest request, FunctionContext context)
	{
		return await ExecuteAsync(async () =>
		{
			var data = await request.ReadFromJsonAsync<DictionaryRootCreateDto>();
			if (data == null)
			{
				throw new BadRequestException("Request body cannot be null.");
			}

			await service.CreateRootAsync(data, context.CancellationToken);
		});
	}

	[Function($"{FUNCTION_NAME}-UpdateRoot")]
	public async Task<IActionResult> UpdateRootAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = $"{ROUTE_PREFIX}/root/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			var data = await request.ReadFromJsonAsync<DictionaryRootUpdateDto>();
			if (data == null)
			{
				throw new BadRequestException("Request body cannot be null.");
			}

			await service.UpdateRootAsync(id, data, context.CancellationToken);
		});
	}

	[Function($"{FUNCTION_NAME}-DeleteRoot")]
	public async Task<IActionResult> DeleteRootAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = $"{ROUTE_PREFIX}/root/{{id:long}}")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(() => service.DeleteRootAsync(id, context.CancellationToken));
	}
}