using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Implements application service for managing projects.
/// </summary>
internal sealed class ProjectApplicationService : BaseApplicationService, IProjectApplicationService
{
	public Task<List<ProjectListDto>> FindAsync(ProjectCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default)
	{
		var request = new ProjectListQueryRequest(criteria, skip, take);
		return Bus.RequestAsync(request, cancellationToken)
		          .ContinueWith(task =>
		          {
			          task.WaitAndUnwrapException(cancellationToken);
			          return TypeAdapter.ProjectedAs<List<ProjectListDto>>(task.Result);
		          }, cancellationToken);
	}

	public Task<int> CountAsync(ProjectCriteriaDto criteria, CancellationToken cancellationToken = default)
	{
		var request = new ProjectCountQueryRequest(criteria);
		return Bus.RequestAsync(request, cancellationToken);
	}

	public Task<ProjectDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var request = new ProjectDetailQueryRequest(id);
		return Bus.RequestAsync(request, cancellationToken)
		          .ContinueWith(task =>
		          {
			          task.WaitAndUnwrapException(cancellationToken);
			          return TypeAdapter.ProjectedAs<ProjectDetailDto>(task.Result);
		          }, cancellationToken);
	}

	public Task<long> CreateAsync(ProjectCreateDto data, CancellationToken cancellationToken = default)
	{
		var command = TypeAdapter.ProjectedAs<ProjectCreateCommand>(data);
		return Bus.SendAsync<ProjectCreateCommand, long>(command, cancellationToken);
	}

	public Task UpdateAsync(long id, ProjectUpdateDto data, CancellationToken cancellationToken = default)
	{
		var command = new ProjectUpdateCommand(id);
		TypeAdapter.ProjectedAs(data, command);
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
	{
		var command = new ProjectDeleteCommand(id);
		return Bus.SendAsync(command, cancellationToken);
	}
}