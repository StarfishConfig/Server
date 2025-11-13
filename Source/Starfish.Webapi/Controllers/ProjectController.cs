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
    public async Task<IActionResult> ListAsync([FromQuery] ProjectCriteriaDto criteria, int skip = 0, int take = 20)
    {
        var result = await service.ListAsync(criteria, skip, take, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Counts the number of projects matching the provided criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpGet("count")]
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
    public async Task<IActionResult> GetAsync(long id)
    {
        var project = await service.GetAsync(id, HttpContext.RequestAborted);
        return Ok(project);
    }
}