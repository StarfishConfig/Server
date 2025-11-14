using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Logging data context for managing logging-related entities.
/// </summary>
[ConnectionString(Name = "LoggingConnection")]
internal sealed class LoggingDataContext : DataContextWithBus<LoggingDataContext>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="LoggingDataContext"/> class.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="provider"></param>
	public LoggingDataContext(DbContextOptions<LoggingDataContext> options, ILazyServiceProvider provider)
		: base(options, provider)
	{
	}
}
