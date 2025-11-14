using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to create a new UserAuthority.
/// </summary>
/// <param name="userId"></param>
/// <param name="provider"></param>
/// <param name="openId"></param>
public sealed class UserAuthorityCreateCommand(long userId, string provider, string openId)
	: Command<long>(userId)
{
	public long EntryId => Item1;

	public string Provider { get; } = provider;

	public string OpenId { get; } = openId;

	public string Name { get; set; }
}