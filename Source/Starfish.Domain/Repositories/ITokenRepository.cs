
namespace Nerosoft.Starfish.Domain;

internal interface ITokenRepository : IBaseRepository<Token, long>
{
    /// <summary>
    /// Find token by key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Token> FindByKeyAsync(string key, CancellationToken cancellationToken = default);
}
