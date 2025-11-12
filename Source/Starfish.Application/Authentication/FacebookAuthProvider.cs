using System.Text.Json.Nodes;
using FluentHttpClient;
using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Authentication provider for Facebook OAuth.
/// </summary>
/// <param name="configuration"></param>
internal sealed class FacebookAuthProvider(IConfiguration configuration) : BaseAuthProvider(configuration)
{
    protected override string Name => nameof(AuthenticationConstant.Provider.Facebook);

    /// <inheritdoc />
    public override async Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default)
    {
        var token = await GetTokenAsync(code, cancellationToken);
        var user = await GetUserAsync(token, cancellationToken);
        var result = new OAuthResult();
        ReadJsonValue(user, "id", id => result.Id = id);
        ReadJsonValue(user, "name", name => result.Nickname = name);
        ReadJsonValue(user, "email", email => result.Email = email);
        //ReadJsonValue(user, "picture", picture => result.AvatarUrl = picture?.GetValue<JsonObject>()?["data"]?.GetValue<string>("url"));
        return result;
    }

    /// <summary>
    /// Get user info from Facebook.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task<JsonObject> GetUserAsync(string token, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        return await client.UsingRoute("https://graph.facebook.com/v12.0/me?fields=id,name")
                           .WithHeader(HeaderUserAgent, UserAgent)
                           .WithHeader(HeaderAccept, JsonContentType)
                           .WithOAuthBearerToken(token)
                           .WithQueryParam("fields", "id,name,email,picture")
                           .GetAsync(cancellationToken)
                           .DeserializeJsonAsync<JsonObject>(cancellationToken);
    }

    /// <summary>
    /// Get access token from Facebook.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BadGatewayException"></exception>
    private async Task<string> GetTokenAsync(string code, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://graph.facebook.com");

        var response = await client.UsingRoute("/v22.0/oauth/access_token")
                                   .WithHeader(HeaderAccept, JsonContentType)
                                   .WithQueryParam("client_id", ClientId)
                                   .WithQueryParam("client_secret", ClientSecret)
                                   .WithQueryParam("code", code)
                                   .WithQueryParam("redirect_uri", RedirectUri)
                                   .GetAsync(cancellationToken)
                                   .DeserializeJsonAsync<JsonObject>(cancellationToken);

        if (response == null)
        {
            throw new BadGatewayException("Failed to get token from Facebook.");
        }

        if (response.TryGetPropertyValue("access_token", out var token) == false || token == null)
        {
            throw new BadGatewayException("Failed to get access token from Facebook.");
        }

        return token.GetValue<string>();
    }
}