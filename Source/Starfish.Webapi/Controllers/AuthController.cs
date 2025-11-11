using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller to handle authentication-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "account")]
public class AuthController(IAuthApplicationService service) : ControllerBase
{
    /// <summary>
    /// Grants an authentication token.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("grant")]
    public async ValueTask<IActionResult> GrantTokenAsync([FromBody] AuthRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await service.GrantAsync(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Refreshes an authentication token.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("refresh")]
    public async ValueTask<IActionResult> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var response = await service.RefreshAsync(token, cancellationToken);
        return Ok(response);
    }
}