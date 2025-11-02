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
    /// <param name="unitOfWork">The <see cref="IUnitOfWorkManager"/> instance.</param>
    protected CommandHandlerBase(IUnitOfWorkManager unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerBase"/> class.
    /// </summary>
    /// <param name="unitOfWork">The <see cref="IUnitOfWorkManager"/> instance.</param>
    /// <param name="factory">The <see cref="IObjectFactory"/> instance.</param>
    protected CommandHandlerBase(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
        : this(unitOfWork)
    {
        Factory = factory;
    }

    /// <summary>
    /// Executes the specified action within a unit of work and returns a command response.
    /// </summary>
    /// <param name="messageId">The unique id of the message to be handled.</param>
    /// <param name="action">The business logic that handles the message.</param>
    /// <returns>An empty command response instance.</returns>
    /// <remarks>This method is commonly used to handle a command which DON'T require handle result or to handle an event message.</remarks>
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
    /// <param name="messageId">The unique id of the message to be handled.</param>
    /// <param name="action">The business logic that handles the message.</param>
    /// <typeparam name="TResult">The execution result type.</typeparam>
    /// <returns>A command response instance with the execution result.</returns>
    /// <remarks>This method is commonly used to handle a command which requires handle result to be returned.</remarks>
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
    /// <param name="action">The business logic that handles the message.</param>
    /// <returns></returns>
    /// <remarks>This method is commonly used to handle a command which DON'T require handle result or to handle an event message.</remarks>
    protected virtual async Task ExecuteAsync([NotNull] Func<Task> action)
    {
        using var uow = UnitOfWork.Begin(true, true);
        await action();
        await uow.CommitAsync();
    }

    /// <summary>
    /// Executes the specified action within a unit of work and invokes the next action with the result.
    /// </summary>
    /// <typeparam name="TResult">The execution result type.</typeparam>
    /// <param name="action">The business logic that handles the message.</param>
    /// <param name="next">The logic to handle the result.</param>
    /// <returns>The execution result.</returns>
    /// <remarks>This method is commonly used to handle a command which requires handle result to be returned.</remarks>
    protected virtual async Task ExecuteAsync<TResult>([NotNull] Func<Task<TResult>> action, Action<TResult> next)
    {
        using var uow = UnitOfWork.Begin(true, true);
        var result = await action();
        await uow.CommitAsync();
        next(result);
    }
}