using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

internal sealed class ConfigurationCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
	: CommandHandlerBase(unitOfWork, factory),
	  IHandler<ConfigurationCreateCommand>,
	  IHandler<ConfigurationUpdateCommand>
{
	public Task HandleAsync(ConfigurationCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<ConfigurationGeneralBusiness>(cancellationToken);
			business.TeamId = message.TeamId;
			business.ProjectId = message.ProjectId;
			business.Name = message.Name;
			business.Description = message.Description;
			business.Secret = message.Secret;

			await business.SaveAsync(false, cancellationToken);

			return business.Id;
		}, context.Response, cancellationToken);
	}

	public Task HandleAsync(ConfigurationUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<ConfigurationGeneralBusiness>(message.EntryId, cancellationToken);
			business.Name = message.Name;
			business.Description = message.Description;

			await business.SaveAsync(false, cancellationToken);
		}, cancellationToken);
	}
}