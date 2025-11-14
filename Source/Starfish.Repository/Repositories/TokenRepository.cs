using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Repository for managing Token entities.
/// </summary>
internal class TokenRepository : BaseRepository<IdentityDataContext, Token, long>, ITokenRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRepository"/> class.
    /// </summary>
    /// <param name="provider"></param>
    public TokenRepository(IContextProvider provider)
        : base(provider)
    {
    }

    /// <inheritdoc />
    public Task<Token> FindByKeyAsync(string key, bool tracking, CancellationToken cancellationToken = default)
    {
        var specification = TokenSpecification.KeyEquals(key);
        var predicate = specification.Satisfy();
        return GetAsync(predicate, tracking, null, cancellationToken);
    }
}