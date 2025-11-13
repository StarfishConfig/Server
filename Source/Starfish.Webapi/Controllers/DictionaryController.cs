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
}
