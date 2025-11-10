using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// The user command handler.
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="factory"></param>
internal class UserCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
    : CommandHandlerBase(unitOfWork, factory),
      IHandler<UserCreateCommand>,
      IHandler<UserUpdateCommand>,
      IHandler<UserPasswordUpdateCommand>,
      IHandler<UserUnlockCommand>
{
    public Task HandleAsync(UserCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.CreateAsync<UserGeneralBusiness>(cancellationToken);
            business.Username = message.Username;
            business.Nickname = message.Nickname;
            business.Password = message.Password;
            business.Email = message.Email;
            business.Phone = message.Phone;
            business.Source = message.Source;
            business.MarkAsInsert();
            await business.SaveAsync(false, cancellationToken);

            return business.Id;
        }, context.Response, cancellationToken);
    }

    /// <summary>
    /// Handle the user update command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<UserGeneralBusiness>(message.UserId, cancellationToken);

            if (message.Nickname is not null)
            {
                business.Nickname = message.Nickname;
            }

            if (message.Email is not null)
            {
                business.Email = message.Email;
            }

            if (message.Phone is not null)
            {
                business.Phone = message.Phone;
            }

            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the user password update command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserPasswordUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<UserPasswordBusiness>(message.UserId, cancellationToken);
            business.Password = message.Password;
            business.ActionType = message.ActionType;
            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the user unlock command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserUnlockCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.CreateAsync<UserLockoutBusiness>(cancellationToken);
            await business.ExecuteAsync(message.UserId, "reset", cancellationToken);
        }, cancellationToken);
    }
}