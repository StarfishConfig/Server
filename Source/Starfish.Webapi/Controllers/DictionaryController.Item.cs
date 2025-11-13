using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Webapi.Controllers;

public partial class DictionaryController
{
    /// <summary>
    /// List dictionary items by dictionary id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/item/list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DictionaryItemListDto>))]
    public async Task<IActionResult> ListItemAsync([FromRoute] long id, [FromQuery] DictionaryItemCriteriaDto criteria, int skip = RequestConstant.Defaults.Skip, int take = RequestConstant.Defaults.Take)
    {
        var result = await service.ListItemAsync(id, criteria, skip, take, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Count dictionary items by dictionary id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/item/count")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> CountItemAsync([FromRoute] long id, [FromQuery] DictionaryItemCriteriaDto criteria)
    {
        var result = await service.CountItemAsync(id, criteria, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Create dictionary item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/item")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AppendItemAsync([FromRoute] long id, [FromBody] DictionaryItemCreateDto data)
    {
        await service.AppendItemAsync(id, data, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Update dictionary item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="itemId"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut("{id:long}/item")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] long id, [FromBody] DictionaryItemUpdateDto data)
    {
        await service.UpdateItemAsync(id, data, HttpContext.RequestAborted);
        return Ok();
    }

    /// <summary>
    /// Delete dictionary items by keys.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    [HttpDelete("{id:long}/item")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteItemAsync([FromRoute] long id, [FromRoute] List<string> keys)
    {
        await service.DeleteItemAsync(id, keys, HttpContext.RequestAborted);
        return Ok();
    }
}
