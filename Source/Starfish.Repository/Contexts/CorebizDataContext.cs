using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for project-related data.
/// </summary>
[ConnectionString(Name = "CorebizConnection")]
internal sealed class CorebizDataContext : DataContextWithBus<CorebizDataContext>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CorebizDataContext"/> class.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="provider"></param>
	public CorebizDataContext(DbContextOptions<CorebizDataContext> options, ILazyServiceProvider provider)
		: base(options, provider)
	{
	}
}