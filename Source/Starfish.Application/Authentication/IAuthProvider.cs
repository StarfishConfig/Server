using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Defines methods for authenticating users via external providers.
/// </summary>
public interface IAuthProvider
{
    Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default);
}

/// <summary>
/// Base class for authentication providers.
/// </summary>
/// <param name="configuration"></param>
internal abstract class BaseAuthProvider(IConfiguration configuration) : IAuthProvider
{
    protected const string HeaderUserAgent = "User-Agent";
    protected const string HeaderAuthorization = "Authorization";
    protected const string HeaderAccept = "Accept";

    protected const string UserAgent = "Starfish";
    protected const string JsonContentType = "application/json";

    protected virtual string ClientId => Configuration.GetValue<string>($"OAuth:{Name}:ClientId");
    protected virtual string ClientSecret => Configuration.GetValue<string>($"OAuth:{Name}:ClientSecret");
    protected virtual string RedirectUri => Configuration.GetValue<string>("OAuth:RedirectUri");

    protected IConfiguration Configuration { get; } = configuration;

    protected abstract string Name { get; }

    public abstract Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default);

    protected virtual void ReadJsonValue(JsonObject json, string key, Action<string> action)
    {
        if (!json.TryGetPropertyValue(key, out var node) || node == null)
        {
            return;
        }

        var valueType = node.GetValueKind();
        switch (valueType)
        {
            case JsonValueKind.Number:
                action(node.GetValue<long>().ToString());
                break;
            case JsonValueKind.String:
                action(node.GetValue<string>());
                break;
            case JsonValueKind.True:
            case JsonValueKind.False:
                action(node.GetValue<bool>().ToString());
                break;
        }
    }
}