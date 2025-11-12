
namespace Nerosoft.Starfish.Domain;

internal interface ITokenRepository : IBaseRepository<Token, long>
{
    /// <summary>
    /// Find token by key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="tracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Token> FindByKeyAsync(string key, bool tracking, CancellationToken cancellationToken = default);
}
