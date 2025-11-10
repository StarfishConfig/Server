using System.Text.Json.Nodes;
using FluentHttpClient;
using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// GitHub authentication provider.
/// </summary>
/// <param name="configuration"></param>
internal sealed class GithubAuthProvider(IConfiguration configuration) : BaseAuthProvider(configuration)
{
    protected override string Name => nameof(AuthenticationConstant.Provider.Github);

    /// <inheritdoc />
    public override async Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default)
    {
        var token = await GetTokenAsync(code, cancellationToken);
        var user = await GetUserAsync(token, cancellationToken);

        var result = new OAuthResult();

        ReadJsonValue(user, "id", id => result.Id = id);
        ReadJsonValue(user, "login", login => result.Username = login);
        ReadJsonValue(user, "name", name => result.Nickname = name);
        ReadJsonValue(user, "email", email => result.Email = email);
        ReadJsonValue(user, "avatar_url", avatarUrl => result.AvatarUrl = avatarUrl);

        return result;
    }

    /// <summary>
    /// Get user info from GitHub.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async ValueTask<JsonObject> GetUserAsync(string token, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.github.com");
        return await client.UsingRoute("/user")
                           .WithHeader(HeaderUserAgent, UserAgent)
                           .WithHeader(HeaderAccept, JsonContentType)
                           .WithOAuthBearerToken(token)
                           .GetAsync(cancellationToken)
                           .DeserializeJsonAsync<JsonObject>(cancellationToken);
    }

    /// <summary>
    /// Get access token from GitHub.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BadGatewayException"></exception>
    private async Task<string> GetTokenAsync(string code, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://github.com");
        var response = await client.UsingRoute("/login/oauth/access_token")
                                   .WithHeader(HeaderAccept, JsonContentType)
                                   .WithQueryParam("client_id", ClientId)
                                   .WithQueryParam("client_secret", ClientSecret)
                                   .WithQueryParam("code", code)
                                   .PostAsync(cancellationToken)
                                   .DeserializeJsonAsync<Dictionary<string, string>>(cancellationToken);

        if (!response.TryGetValue("access_token", out var token))
        {
            throw new BadGatewayException();
        }

        return token;
    }
}