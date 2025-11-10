using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to remove a UserAuthority.
/// </summary>
internal sealed class UserAuthorityRemoveCommand(long userId, string provider, string openId)
    : Command<long>(userId)
{
    public long UserId => Item1;

    public string Provider { get; set; } = provider;

    public string OpenId { get; set; } = openId;
}