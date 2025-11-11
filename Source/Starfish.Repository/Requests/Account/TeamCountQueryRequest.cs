using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to get the count of teams matching a keyword.
/// </summary>
internal sealed class TeamCountQueryRequest : TeamSearchRequest, IRequest<int>;