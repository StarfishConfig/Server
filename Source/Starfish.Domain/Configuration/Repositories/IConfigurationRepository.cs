namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines repository interface for <see cref="Configuration"/> aggregate.
/// </summary>
public interface IConfigurationRepository : IBaseRepository<Configuration, long>
{
	/// <summary>
	/// Gets the configuration items by configuration id.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<ConfigurationItem>> GetItemsAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Checks if a specific version of the configuration exists.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="version"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsVersionAsync(long id, string version, CancellationToken cancellationToken = default);

	/// <summary>
	/// Inserts a new configuration revision.
	/// </summary>
	/// <param name="revision"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task InsertRevisionAsync(ConfigurationRevision revision, CancellationToken cancellationToken = default);

	/// <summary>
	/// Inserts a new configuration archive.
	/// </summary>
	/// <param name="archive"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task InsertArchiveAsync(ConfigurationArchive archive, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing configuration archive.
	/// </summary>
	/// <param name="archive"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateArchiveAsync(ConfigurationArchive archive, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a configuration archive by id.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<ConfigurationArchive> GetArchiveAsync(long id, bool tracking, CancellationToken cancellationToken = default);
}