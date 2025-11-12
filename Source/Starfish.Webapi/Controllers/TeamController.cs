using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing team-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = ApiGroupConstants.Account)]
public partial class TeamController(ITeamApplicationService service) : ControllerBase
{
    /// <summary>
    /// Finds teams based on the provided criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] TeamCriteriaDto criteria, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var teams = await service.ListAsync(criteria, skip, take, HttpContext.RequestAborted);
        return Ok(teams);
    }

    /// <summary>
    /// Counts teams based on the provided criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpGet("count")]
    public async Task<IActionResult> CountAsync([FromQuery] TeamCriteriaDto criteria)
    {
        var count = await service.CountAsync(criteria, HttpContext.RequestAborted);
        return Ok(count);
    }

    /// <summary>
    /// Gets the details of a specific team by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var team = await service.GetAsync(id, HttpContext.RequestAborted);
        return Ok(team);
    }

    /// <summary>
    /// Creates a new team.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TeamCreateDto dto)
    {
        var id = await service.CreateAsync(dto, HttpContext.RequestAborted);
        return Ok(id);
    }

    /// <summary>
    /// Updates an existing team.
    /// </summary>
    /// <param name="id">The team identifier to be updated.</param>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] TeamUpdateDto data)
    {
        await service.UpdateAsync(id, data, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Deletes a team by its identifier.
    /// </summary>
    /// <param name="id">The team identifier to be deleted.</param>
    /// <returns></returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Transfers a team to another user.
    /// </summary>
    /// <param name="id">The team identifier to transfer.</param>
    /// <param name="data">The request data that contains the user id of the new team owner.</param>
    /// <returns></returns>
    [HttpPost("{id:long}/transfer")]
    public async Task<IActionResult> TransferAsync(long id, [FromBody] TeamTransferDto data)
    {
        await service.TransferAsync(id, data, HttpContext.RequestAborted);
        return Ok();
    }
}