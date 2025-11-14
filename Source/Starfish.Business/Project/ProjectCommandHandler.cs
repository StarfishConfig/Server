using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

internal sealed class ProjectCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
	: CommandHandlerBase(unitOfWork, factory),
	IHandler<ProjectCreateCommand>,
	IHandler<ProjectUpdateCommand>,
	IHandler<ProjectDeleteCommand>
{
	public Task HandleAsync(ProjectCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<ProjectGeneralBusiness>(cancellationToken);

			business.TeamId = message.TeamId;
			business.Name = message.Name;
			business.Description = message.Description;
			business.Url = message.Url;
			business.Image = message.Image;

			business.MarkAsInsert();

			await business.SaveAsync(false, cancellationToken);

			return business.Id;

		}, context.Response, cancellationToken);
	}

	public Task HandleAsync(ProjectUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ProjectGeneralBusiness>(message.EntryId, cancellationToken);

			business.Name = message.Name;
			business.Description = message.Description;
			business.Url = message.Url;
			business.Image = message.Image;

			business.MarkAsInsert();

			await business.SaveAsync(true, cancellationToken);
		}, cancellationToken);
	}

	public Task HandleAsync(ProjectDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ProjectGeneralBusiness>(message.EntryId, cancellationToken);

			business.MarkAsDelete();

			await business.SaveAsync(true, cancellationToken);
		}, cancellationToken);
	}
}