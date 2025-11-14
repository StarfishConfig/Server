using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for system-related data.
/// </summary>
[ConnectionString(Name = "SystemConnection")]
internal class SystemDataContext : DataContextWithBus<SystemDataContext>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SystemDataContext"/> class.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="provider"></param>
	public SystemDataContext(DbContextOptions<SystemDataContext> options, ILazyServiceProvider provider)
		: base(options, provider)
	{
	}
}