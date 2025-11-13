
namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines repository interface for <see cref="Token"/> aggregate.
/// </summary>
public interface ITokenRepository : IBaseRepository<Token, long>
{
    /// <summary>
    /// Find token by key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="tracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Token> FindByKeyAsync(string key, bool tracking, CancellationToken cancellationToken = default);
}
