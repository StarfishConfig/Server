using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing user-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "account")]
[Authorize]
public class UserController(IUserApplicationService service) : ControllerBase
{
    /// <summary>
    /// Lists users based on specified criteria with pagination.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] UserCriteriaDto criteria, int skip = 0, int take = 20)
    {
        var result = await service.FindAsync(criteria, skip, take, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Counts users based on specified criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpGet("count")]
    public async Task<IActionResult> CountAsync([FromQuery] UserCriteriaDto criteria)
    {
        var count = await service.CountAsync(criteria, HttpContext.RequestAborted);
        return Ok(count);
    }

    /// <summary>
    /// Gets user detail by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var user = await service.GetAsync(id, HttpContext.RequestAborted);
        return Ok(user);
    }

    /// <summary>
    /// Updates the password of an existing user.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id:long}/password")]
    public async Task<IActionResult> ResetPasswordAsync(long id)
    {
        var password = await service.ResetPasswordAsync(id, HttpContext.RequestAborted);
        return Ok(password);
    }

    /// <summary>
    /// Unlocks a locked user account.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id:long}/unlock")]
    public async Task<IActionResult> UnlockAsync(long id)
    {
        await service.UnlockAsync(id, HttpContext.RequestAborted);
        return Ok();
    }
}