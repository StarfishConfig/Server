using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;
using Nerosoft.Starfish.Toolkit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing account-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = ApiGroupConstants.Identity)]
[Authorize]
public class AccountController(IUserApplicationService service) : ControllerBase
{
	/// <summary>
	/// Creates a new user.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	[AllowAnonymous]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResultDto<long>))]
	public async Task<IActionResult> CreateAsync([FromBody] UserCreateDto data)
	{
		var userId = await service.CreateAsync(data, HttpContext.RequestAborted);
		return StatusCode(StatusCodes.Status201Created, new CreatedResultDto<long>(userId));
	}

	/// <summary>
	/// Gets the profile of the specified user.
	/// </summary>
	/// <returns></returns>
	[HttpGet("profile")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileDto))]
	public async Task<IActionResult> GetProfileAsync()
	{
		var profile = await service.GetProfileAsync(HttpContext.RequestAborted);
		return Ok(profile);
	}

	/// <summary>
	/// Changes the password for a user.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("password/change")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ChangePasswordAsync([FromBody] UserPasswordChangeDto data)
	{
		await service.ChangePasswordAsync(data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// Resets the password for a user.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("password/reset")]
	[AllowAnonymous]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ResetPasswordAsync([FromBody] UserPasswordResetDto data)
	{
		await service.ResetPasswordAsync(data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// Changes the phone number for current user.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPatch, HttpPost]
	[Route("email")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> UpdateEmailAsync([FromBody] UserEmailUpdateDto data)
	{
		await service.UpdateEmailAsync(data.Email, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// Changes the phone number for current user.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPatch, HttpPost]
	[Route("phone")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> UpdatePhoneAsync([FromBody] UserPhoneUpdateDto data)
	{
		await service.UpdatePhoneAsync(data.Phone, HttpContext.RequestAborted);
		return Ok();
	}
}