using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to get team details by ID.
/// </summary>
/// <param name="Id"></param>
internal record TeamDetailQueryRequest(long Id) : IRequest<Team>;
