using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface IProjectApplicationService : IApplicationService
{
    Task<List<ProjectListDto>> ListAsync(ProjectCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ProjectCriteriaDto criteria, CancellationToken cancellationToken = default);

    Task<ProjectDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);
}