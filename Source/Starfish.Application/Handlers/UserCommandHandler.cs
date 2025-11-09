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
      IHandler<UserPasswordChangeCommand>
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

    public Task HandleAsync(UserUpdateCommand message, MessageContext context, CancellationToken cancellationToken = new CancellationToken())
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
    
    public Task HandleAsync(UserPasswordChangeCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<UserPasswordBusiness>(message.UserId, cancellationToken);
            business.Password = message.Password;
            business.ChangeType = message.ChangeType;
            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    
}