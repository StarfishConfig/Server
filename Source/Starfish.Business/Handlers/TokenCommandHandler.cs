using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Command handler for token-related commands.
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="factory"></param>
internal sealed class TokenCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
    : CommandHandlerBase(unitOfWork, factory),
      IHandler<TokenCreateCommand>,
      IHandler<TokenRevokeCommand>
{
    public Task HandleAsync(TokenCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var token = await Factory.CreateAsync<TokenGeneralBusiness>(cancellationToken);
            token.Type = message.Type;
            token.Subject = message.Subject;
            token.AccessToken = message.Token;
            token.Issued = message.Issued;
            token.Expires = message.Expires;

            await token.SaveAsync(false, cancellationToken);
        }, cancellationToken);
    }

    public Task HandleAsync(TokenRevokeCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}