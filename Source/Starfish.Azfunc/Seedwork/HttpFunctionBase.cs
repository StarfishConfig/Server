using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc;

/// <summary>
/// The base class for HTTP-triggered Azure Functions.
/// </summary>
/// <param name="logger"></param>
/// <typeparam name="T"></typeparam>
public abstract class HttpFunctionBase<T>(ILoggerFactory logger)
    where T : HttpFunctionBase<T>
{
    private ILogger<T> _logger;

    protected virtual ILogger<T> Logger => _logger ??= logger.CreateLogger<T>();

    /// <summary>
    /// Executes the specified asynchronous action and returns an OkObjectResult with the result.
    /// </summary>
    /// <param name="action"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    protected virtual async Task<IActionResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
    {
        var result = await action();
        return Ok(result);
    }

    /// <summary>
    /// Executes the specified asynchronous action and returns an OkResult.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    protected virtual async Task<IActionResult> ExecuteAsync(Func<Task> action)
    {
        await action();
        return Ok();
    }

    /// <summary>
    /// Returns an OkObjectResult with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual IActionResult Ok(object value)
    {
        return new OkObjectResult(value);
    }

    /// <summary>
    /// Returns an OkResult.
    /// </summary>
    /// <returns></returns>
    protected virtual IActionResult Ok()
    {
        return new OkResult();
    }
}