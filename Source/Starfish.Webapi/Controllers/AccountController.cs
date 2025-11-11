using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing account-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = ApiGroupConstants.Account)]
[Authorize]
public class AccountController(IUserApplicationService service) : ControllerBase
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async ValueTask<IActionResult> CreateAsync([FromBody] UserCreateRequestDto data, CancellationToken cancellationToken = default)
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
