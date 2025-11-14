using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Identity data context for managing identity-related entities.
/// </summary>
[ConnectionString(Name = "IdentityConnection")]
internal sealed class IdentityDataContext : DataContextWithBus<IdentityDataContext>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="IdentityDataContext"/> class.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="provider"></param>
	public IdentityDataContext(DbContextOptions<IdentityDataContext> options, ILazyServiceProvider provider)
		: base(options, provider)
	{
	}
}