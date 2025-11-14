using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for project-related data.
/// </summary>
[ConnectionString(Name = "PrimaryConnection")]
internal sealed class PrimaryDataContext : DataContextWithBus<PrimaryDataContext>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PrimaryDataContext"/> class.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="provider"></param>
	public PrimaryDataContext(DbContextOptions<PrimaryDataContext> options, ILazyServiceProvider provider)
		: base(options, provider)
	{
	}
}