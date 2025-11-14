using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

internal sealed class ProjectRequestHandler(IProjectRepository repository) : IHandler<ProjectListQueryRequest>,
	IHandler<ProjectCountQueryRequest>,
											  IHandler<ProjectDetailQueryRequest>
{
	public Task HandleAsync(ProjectListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var specification = ProjectSpecification.ApplyCriteria(message.Criteria);
		var predicate = specification.Satisfy();
		return repository.FindAsync(predicate, null, message.Skip, message.Take, cancellationToken)
				   .ContinueWith(task => context.Response(task.Result));
	}

	public Task HandleAsync(ProjectCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var specification = ProjectSpecification.ApplyCriteria(message.Criteria);
		var predicate = specification.Satisfy();
		return repository.CountAsync(predicate, null, cancellationToken)
				   .ContinueWith(task => context.Response(task.Result), cancellationToken);
	}

	public Task HandleAsync(ProjectDetailQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return repository.GetAsync(message.Id, false, [], cancellationToken)
				   .ContinueWith(task => context.Response(task.Result), cancellationToken);
	}
}
