using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing authentication tokens.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = ApiGroupConstants.Identity)]
public class TokenController(IAuthApplicationService service) : ControllerBase
{
	/// <summary>
	/// Grants an authentication token.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost("grant")]
	public async Task<IActionResult> GrantAsync([FromBody] TokenGrantRequestDto request, CancellationToken cancellationToken = default)
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
	public async Task<IActionResult> RefreshAsync(string token, CancellationToken cancellationToken = default)
	{
		var response = await service.RefreshAsync(token, cancellationToken);
		return Ok(response);
	}

	/// <summary>
	/// Revokes an authentication token.
	/// </summary>
	/// <param name="token"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost("revoke")]
	public async Task<IActionResult> RevokeAsync(string token, CancellationToken cancellationToken = default)
	{
		await service.RevokeAsync(token, cancellationToken);
		return Ok();
	}
}