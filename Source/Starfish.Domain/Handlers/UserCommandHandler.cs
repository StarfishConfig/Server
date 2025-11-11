using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

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
      IHandler<UserFailureResetCommand>,
      IHandler<UserFailureIncreaseCommand>,
      IHandler<UserAuthorityCreateCommand>,
      IHandler<UserAuthorityRemoveCommand>
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
    public Task HandleAsync(UserFailureResetCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            await Factory.ExecuteAsync<UserLockoutBusiness>(message.UserId, "reset", cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the user failure increase command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserFailureIncreaseCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            await Factory.ExecuteAsync<UserLockoutBusiness>(message.Username, "increase", cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the user authority create command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserAuthorityCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<UserAuthorityBusiness>(message.UserId, cancellationToken);
            business.Provider = message.Provider;
            business.OpenId = message.OpenId;
            business.Name = message.Name;
            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the user authority remove command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserAuthorityRemoveCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<UserAuthorityBusiness>(message.UserId, cancellationToken);
            business.Provider = message.Provider;
            business.OpenId = message.OpenId;
            business.MarkAsDelete();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }
}