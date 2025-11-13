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
      IHandler<TeamUpdateCommand>,
      IHandler<TeamDeleteCommand>,
      IHandler<TeamTransferCommand>,
      IHandler<TeamProjectCountIncreaseCommand>,
      IHandler<TeamProjectCountDecreaseCommand>,
      IHandler<TeamMemberAppendCommand>,
      IHandler<TeamMemberRemoveCommand>
{
    /// <summary>
    /// Handle the team create command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Handle the team update command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task HandleAsync(TeamUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<TeamGeneralBusiness>(message.TeamId, cancellationToken);
            business.Name = message.Name;
            business.Description = message.Description;
            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the team delete command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task HandleAsync(TeamDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<TeamGeneralBusiness>(message.TeamId, cancellationToken);
            business.MarkAsDelete();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Handle the team transfer command.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(TeamTransferCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(() => Factory.ExecuteAsync<TeamTransferBusiness>(message.TeamId, message.UserId, cancellationToken), cancellationToken);
    }

    public Task HandleAsync(TeamProjectCountIncreaseCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(() => Factory.ExecuteAsync<TeamProjectCountChangeBusiness>(message.TeamId, "+", cancellationToken), cancellationToken);
    }

    public Task HandleAsync(TeamProjectCountDecreaseCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(() => Factory.ExecuteAsync<TeamProjectCountChangeBusiness>(message.TeamId, "-", cancellationToken), cancellationToken);
    }

    public Task HandleAsync(TeamMemberAppendCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<TeamMemberBusiness>(message.TeamId, cancellationToken);
            business.UserIds = message.UserIds;
            business.MarkAsUpdate();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }

    public Task HandleAsync(TeamMemberRemoveCommand message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var business = await Factory.FetchAsync<TeamMemberBusiness>(message.TeamId, cancellationToken);
            business.UserIds = message.UserIds;
            business.Reason = message.Reason;
            business.MarkAsDelete();
            await business.SaveAsync(true, cancellationToken);
        }, cancellationToken);
    }
}