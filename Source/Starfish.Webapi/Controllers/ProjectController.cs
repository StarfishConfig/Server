using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing project-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = ApiGroupConstants.Project)]
[Authorize]
public class ProjectController(IProjectApplicationService service) : ControllerBase
{
	/// <summary>
	/// Lists projects based on the provided criteria with pagination support.
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <returns></returns>
	[HttpGet("list")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProjectListDto>))]
	public async Task<IActionResult> ListAsync([FromQuery] ProjectCriteriaDto criteria, int skip = 0, int take = 20)
	{
		var result = await service.FindAsync(criteria, skip, take, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// Counts the number of projects matching the provided criteria.
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
	public async Task<IActionResult> CountAsync([FromQuery] ProjectCriteriaDto criteria)
	{
		var count = await service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(count);
	}

	/// <summary>
	/// Gets the details of a specific project by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id:long}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectDetailDto))]
	public async Task<IActionResult> GetAsync(long id)
	{
		var project = await service.GetAsync(id, HttpContext.RequestAborted);
		return Ok(project);
	}

	/// <summary>
	/// Creates a new project.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResultDto<long>))]
	public async Task<IActionResult> CreateAsync([FromBody] ProjectCreateDto data)
	{
		var projectId = await service.CreateAsync(data, HttpContext.RequestAborted);
		return StatusCode(StatusCodes.Status201Created, new CreatedResultDto<long>(projectId));
	}

	/// <summary>
	/// Updates an existing project identified by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id:long}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> UpdateAsync(long id, [FromBody] ProjectUpdateDto data)
	{
		await service.UpdateAsync(id, data, HttpContext.RequestAborted);
		return StatusCode(StatusCodes.Status204NoContent);
	}

	/// <summary>
	/// Deletes a project by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("{id:long}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> DeleteAsync(long id)
	{
		await service.DeleteAsync(id, HttpContext.RequestAborted);
		return StatusCode(StatusCodes.Status204NoContent);
	}
}