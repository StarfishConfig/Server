using System.Text.Json.Nodes;
using FluentHttpClient;
using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Microsoft authentication provider.
/// </summary>
/// <param name="configuration"></param>
internal sealed class MicrosoftAuthProvider(IConfiguration configuration) : BaseAuthProvider(configuration)
{
    protected override string Name => nameof(AuthenticationConstant.Provider.Microsoft);

    /// <inheritdoc />
    public override async Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default)
    {
        var token = await GetTokenAsync(code, cancellationToken);
        var user = await GetUserAsync(token, cancellationToken);

        var result = new OAuthResult();

        ReadJsonValue(user, "id", id => result.Id = id);
        ReadJsonValue(user, "userPrincipalName", login => result.Username = login);
        ReadJsonValue(user, "displayName", name => result.Nickname = name);
        ReadJsonValue(user, "email", email => result.Email = email);
        ReadJsonValue(user, "mobilePhone", avatarUrl => result.Phone = avatarUrl);

        return result;
    }

    /// <summary>
    /// Get user info from Microsoft Graph API.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task<JsonObject> GetUserAsync(string token, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        return await client.UsingRoute("https://graph.microsoft.com/v1.0/me")
                           .WithHeader(HeaderUserAgent, UserAgent)
                           .WithHeader(HeaderAccept, JsonContentType)
                           .WithOAuthBearerToken(token)
                           .GetAsync(cancellationToken)
                           .DeserializeJsonAsync<JsonObject>(cancellationToken);
    }

    /// <summary>
    /// Get access token from Microsoft.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BadGatewayException"></exception>
    private async Task<string> GetTokenAsync(string code, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://login.microsoftonline.com");

        var formContent = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "code", code },
            { "redirect_uri", RedirectUri },
            { "grant_type", "authorization_code" },
            { "scope", "User.Read Mail.Read" }
        };

        var response = await client.UsingRoute("/consumers/oauth2/v2.0/token")
                                   .WithHeader(HeaderAccept, JsonContentType)
                                   .WithContent(new FormUrlEncodedContent(formContent))
                                   .PostAsync(cancellationToken)
                                   .DeserializeJsonAsync<JsonObject>(cancellationToken);
        if (response == null)
        {
            throw new BadGatewayException("Failed to get token from Microsoft.");
        }

        if (response.TryGetPropertyValue("access_token", out var token) == false || token == null)
        {
            throw new BadGatewayException("Failed to get access token from Microsoft.");
        }

        return token.GetValue<string>();
    }
}