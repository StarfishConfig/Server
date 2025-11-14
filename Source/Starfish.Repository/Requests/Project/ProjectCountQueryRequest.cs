using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines the request to count projects based on criteria
/// </summary>
/// <param name="Criteria"></param>
internal sealed record ProjectCountQueryRequest(ProjectCriteriaDto Criteria) : IRequest<int>;
