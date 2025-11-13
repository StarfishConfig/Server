using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// Controller for managing dictionary-related operations.
/// </summary>
/// <param name="service"></param>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public partial class DictionaryController(IDictionaryApplicationService service) : ControllerBase
{
    /// <summary>
    /// Lists dictionary root entries based on specified criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DictionaryRootListDto>))]
    public async Task<IActionResult> ListRootAsync([FromQuery] DictionaryRootCriteriaDto criteria, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
    {
        var result = await service.ListRootAsync(criteria, skip, take, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Counts dictionary root entries based on specified criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> CountRootAsync([FromQuery] DictionaryRootCriteriaDto criteria)
    {
        var result = await service.CountRootAsync(criteria, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Looks up dictionary entries for the provided codes.
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    [HttpPost("lockup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DictionaryLookupDto>))]
    [AllowAnonymous]
    public async Task<IActionResult> LookupAsync([FromBody] List<string> codes)
    {
        var result = await service.LookupAsync(codes, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new dictionary root entry.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRootAsync([FromBody] DictionaryRootCreateDto data)
    {
        await service.CreateRootAsync(data, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Updates an existing dictionary root entry.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateRootAsync([FromRoute] long id, [FromBody] DictionaryRootUpdateDto data)
    {
        await service.UpdateRootAsync(id, data, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Deletes a dictionary root entry by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRootAsync([FromRoute] long id)
    {
        await service.DeleteRootAsync(id, HttpContext.RequestAborted);
        return Ok();
    }
}
