using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

public partial class TeamController
{
    /// <summary>
    /// Gets a list of team members based on the provided criteria.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/member/list")]
    public async Task<IActionResult> FindMemberAsync(long id, [FromQuery] TeamMemberCriteriaDto criteria, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var members = await service.FindMemberAsync(id, criteria, skip, take, HttpContext.RequestAborted);
        return Ok(members);
    }

    /// <summary>
    /// Gets the count of team members based on the provided criteria.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/member/count")]
    public async Task<IActionResult> CountMemberAsync(long id, [FromQuery] TeamMemberCriteriaDto criteria)
    {
        var count = await service.CountMemberAsync(id, criteria, HttpContext.RequestAborted);
        return Ok(count);
    }

    /// <summary>
    /// Appends members to the team.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/member")]
    public async Task<IActionResult> AppendMemberAsync(long id, [FromBody] List<long> userIds)
    {
        await service.AppendMemberAsync(id, userIds, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Removes members from the team.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    [HttpDelete("{id:long}/member")]
    public async Task<IActionResult> RemoveMemberAsync(long id, [FromBody] List<long> userIds)
    {
        await service.RemoveMemberAsync(id, userIds, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Quits the team.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <returns></returns>
    [HttpPost("{id:long}/quit")]
    public async Task<IActionResult> QuitAsync(long id)
    {
        await service.QuitAsync(id, HttpContext.RequestAborted);
        return Ok();
    }
}