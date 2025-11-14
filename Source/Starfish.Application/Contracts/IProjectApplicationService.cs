using Microsoft.AspNetCore.Authorization;
using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Application service for managing projects.
/// </summary>
public interface IProjectApplicationService : IApplicationService
{
	/// <summary>
	/// Find projects by criteria with pagination.
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize]
	Task<List<ProjectListDto>> FindAsync(ProjectCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

	/// <summary>
	/// Count projects by criteria.
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize]
	Task<int> CountAsync(ProjectCriteriaDto criteria, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get project detail by id.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize]
	Task<ProjectDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Create a new project.
	/// </summary>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize]
	Task<long> CreateAsync(ProjectCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// Update an existing project.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize]
	Task UpdateAsync(long id, ProjectUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// Delete a project by id.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize]
	Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}