using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Command handler for team-related commands.
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="factory"></param>
internal sealed class TeamCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
    : CommandHandlerBase(unitOfWork, factory),
    IHandler<TeamCreateCommand>,
    IHandler<TeamUpdateCommand>
{
    public Task HandleAsync(TeamCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.CreateAsync<TeamGeneralBusiness>(cancellationToken);
            business.Name = message.Name;
            business.Description = message.Description;
            await business.SaveAsync(false, cancellationToken);
            return business.Id;
        }, context.Response, cancellationToken);
    }

    public Task HandleAsync(TeamUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
