using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to query user profile by user ID.
/// </summary>
/// <param name="UserId"></param>
internal record UserProfileQueryRequest(long UserId) : IRequest<User>;