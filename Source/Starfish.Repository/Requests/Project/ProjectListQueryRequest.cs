using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

internal sealed record ProjectListQueryRequest(ProjectCriteriaDto Criteria, int Skip, int Take)
	: IRequest<IList<Project>>;
