using IdentityModel;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business logic for revoking tokens.
/// </summary>
internal sealed class TokenRevokeBusiness : CommandObjectBase<TokenRevokeBusiness>
{
    private ITokenRepository Repository => LazyServiceProvider.GetRequiredService<ITokenRepository>();

    [FactoryExecute]
    public async Task ExecuteAsync(string token, string reason, CancellationToken cancellationToken = default)
    {
        var key = token.ToSha256();
        var entity = await Repository.FindByKeyAsync(key, true, cancellationToken);
        entity.Revoke(reason);
        await Repository.UpdateAsync(entity, true, cancellationToken);
    }
}