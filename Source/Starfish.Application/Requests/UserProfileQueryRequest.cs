using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application.Requests;

/// <summary>
/// Request to query user profile by user ID.
/// </summary>
/// <param name="UserId"></param>
internal record UserProfileQueryRequest(long UserId) : IRequest<UserProfileDto>;