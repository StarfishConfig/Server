using System.Text.Json.Nodes;
using FluentHttpClient;
using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Google authentication provider.
/// </summary>
/// <param name="configuration"></param>
internal sealed class GoogleAuthProvider(IConfiguration configuration) : BaseAuthProvider(configuration)
{
    protected override string Name => nameof(AuthenticationConstant.Provider.Google);

    /// <inheritdoc />
    public override async Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default)
    {
        var token = await GetTokenAsync(code, cancellationToken);
        var user = await GetUserAsync(token, cancellationToken);

        var result = new OAuthResult();

        ReadJsonValue(user, "sub", id => result.Id = id);
        ReadJsonValue(user, "email", email => result.Username = email);
        ReadJsonValue(user, "name", name => result.Nickname = name);
        ReadJsonValue(user, "picture", avatarUrl => result.AvatarUrl = avatarUrl);

        return result;
    }

    /// <summary>
    /// Get user info from Google.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task<JsonObject> GetUserAsync(string token, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        return await client.UsingRoute("https://www.googleapis.com/oauth2/v3/userinfo")
                           .WithHeader(HeaderUserAgent, UserAgent)
                           .WithHeader(HeaderAccept, JsonContentType)
                           .WithOAuthBearerToken(token)
                           .GetAsync(cancellationToken)
                           .DeserializeJsonAsync<JsonObject>(cancellationToken);
    }

    /// <summary>
    /// Get access token from Google.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BadGatewayException"></exception>
    private async Task<string> GetTokenAsync(string code, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://oauth2.googleapis.com");

        var formContent = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "code", code },
            { "redirect_uri", RedirectUri },
            { "grant_type", "authorization_code" }
        };

        var response = await client.UsingRoute("/token")
                                   .WithHeader(HeaderAccept, JsonContentType)
                                   .WithContent(new FormUrlEncodedContent(formContent))
                                   .PostAsync(cancellationToken)
                                   .DeserializeJsonAsync<JsonObject>(cancellationToken);
        if (response == null)
        {
            throw new BadGatewayException("Failed to get token from Google.");
        }

        if (response.TryGetPropertyValue("access_token", out var token) == false || token == null)
        {
            throw new BadGatewayException("Failed to get access token from Google.");
        }

        return token.GetValue<string>();
    }
}