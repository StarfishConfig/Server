using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Azfunc.Functions;

public partial class TeamFunction
{
	/// <summary>
	/// Finds the members of the team.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <returns></returns>
	[Function($"{ROUTE_PREFIX}-FindMember")]
	public async Task<IActionResult> FindMemberAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/{{id:long}}/member/list")] HttpRequest request, FunctionContext context, long id, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<TeamMemberCriteriaDto>();
			var members = await service.FindMemberAsync(id, criteria, skip, take, context.CancellationToken);
			return members;
		});
	}

	/// <summary>
	/// Counts the members of the team.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	[Function($"{ROUTE_PREFIX}-CountMember")]
	public async Task<IActionResult> CountMemberAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{ROUTE_PREFIX}/{{id:long}}/member/count")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			var criteria = context.GetQueryParameter<TeamMemberCriteriaDto>();
			var count = await service.CountMemberAsync(id, criteria, context.CancellationToken);
			return count;
		});
	}

	/// <summary>
	/// Appends members to the team.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{ROUTE_PREFIX}-AppendMember")]
	public async Task<IActionResult> AppendMemberAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/{{id:long}}/member")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			var userIds = await request.ReadFromJsonAsync<List<long>>();
			if (userIds == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.AppendMemberAsync(id, userIds, context.CancellationToken);
			return new OkResult();
		});
	}

	/// <summary>
	/// Removes members from the team.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="BadRequestException"></exception>
	[Function($"{ROUTE_PREFIX}-RemoveMember")]
	public async Task<IActionResult> RemoveMemberAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = $"{ROUTE_PREFIX}/{{id:long}}/member")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			var userIds = await request.ReadFromJsonAsync<List<long>>();
			if (userIds == null)
			{
				throw new BadRequestException("Invalid request body.");
			}
			await service.RemoveMemberAsync(id, userIds, context.CancellationToken);
			return new OkResult();
		});
	}

	/// <summary>
	/// Quits the team.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	[Function($"{ROUTE_PREFIX}-Quit")]
	public async Task<IActionResult> QuitMemberAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{ROUTE_PREFIX}/{{id:long}}/member/quit")] HttpRequest request, FunctionContext context, long id)
	{
		return await ExecuteAsync(async () =>
		{
			await service.QuitAsync(id, context.CancellationToken);
			return new OkResult();
		});
	}
}
