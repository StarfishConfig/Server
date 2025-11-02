using System.Diagnostics.CodeAnalysis;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command handler base class.
/// </summary>
public abstract class CommandHandlerBase
{
    /// <summary>
    /// Gets the unit of work manager.
    /// </summary>
    protected virtual IUnitOfWorkManager UnitOfWork { get; }

    /// <summary>
    /// Gets the object factory.
    /// </summary>
    protected virtual IObjectFactory Factory { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerBase"/> class.
    /// </summary>
    /// <param name="unitOfWork"></param>
    protected CommandHandlerBase(IUnitOfWorkManager unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerBase"/> class.
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="factory"></param>
    protected CommandHandlerBase(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
        : this(unitOfWork)
    {
        Factory = factory;
    }

    /// <summary>
    /// Executes the specified action within a unit of work and returns a command response.
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    protected virtual async Task<CommandResponse> ExecuteAsync(string messageId, [NotNull] Func<Task> action)
    {
        var response = new CommandResponse(messageId);
        try
        {
            using (var uow = UnitOfWork.Begin(true, true))
            {
                await action();
                await uow.CommitAsync();
            }

            response.Success();
        }
        catch (Exception exception)
        {
            response.Failure(exception);
        }

        return response;
    }

    /// <summary>
    /// Executes the specified action within a unit of work and returns a command response with a result.
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="action"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    protected virtual async Task<CommandResponse<TResult>> ExecuteAsync<TResult>(string messageId, [NotNull] Func<Task<TResult>> action)
    {
        var response = new CommandResponse<TResult>(messageId);
        try
        {
            TResult result;
            using (var uow = UnitOfWork.Begin(true, true))
            {
                result = await action();
                await uow.CommitAsync();
            }

            response.Success(result);
        }
        catch (Exception exception)
        {
            response.Failure(exception);
        }

        return response;
    }

    /// <summary>
    /// Executes the specified action within a unit of work.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    protected virtual async Task ExecuteAsync([NotNull] Func<Task> action)
    {
        using var uow = UnitOfWork.Begin(true, true);
        await action();
        await uow.CommitAsync();
    }

    /// <summary>
    /// Executes the specified action within a unit of work and invokes the next action with the result.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    protected virtual async Task ExecuteAsync<TResult>([NotNull] Func<Task<TResult>> action, Action<TResult> next)
    {
        using var uow = UnitOfWork.Begin(true, true);
        var result = await action();
        await uow.CommitAsync();
        next(result);
    }
}