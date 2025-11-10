using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing user-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserApplicationService service) : ControllerBase
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async ValueTask<IActionResult> CreateAsync([FromBody] UserCreateDto data, CancellationToken cancellationToken = default)
    {
        var userId = await service.CreateAsync(data, cancellationToken);
        return Ok(userId);
    }

    /// <summary>
    /// Gets the profile of the specified user.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("profile")]
    public async ValueTask<IActionResult> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var profile = await service.GetProfileAsync(cancellationToken);
        return Ok(profile);
    }
}