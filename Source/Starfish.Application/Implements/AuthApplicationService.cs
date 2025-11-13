using IdentityModel;
using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Shared;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// The authentication application service implementation.
/// </summary>
internal class AuthApplicationService(IConfiguration configuration) : BaseApplicationService, IAuthApplicationService
{
    /// <inheritdoc />
    public async Task<TokenGrantResultDto> GrantAsync(TokenGrantRequestDto data, CancellationToken cancellationToken = default)
    {
        var request = await GetRequestAsync();

        var events = new List<ApplicationEvent>();

        try
        {
            var user = await Bus.RequestAsync(request, cancellationToken);
            var result = GenerateAccessToken(user);
            events.Add(new UserAuthSucceedEvent
            {
                AuthType = data.Provider,
                RefreshToken = result.RefreshToken,
                UserId = result.UserId,
                Username = result.Username,
                TokenIssueTime = DateTimeHelper.GetDateTimeFromUnixTime(result.IssueAt)
            });
            return result;
        }
        catch (Exception exception)
        {
            events.Add(new UserAuthFailedEvent
            {
                AuthType = data.Provider,
                Data = new Dictionary<string, string>
                {
                    { "Username", data.Username ?? string.Empty },
                    { "Password", data.Password != null ? "******" : string.Empty },
                },
                Error = exception.Message,
            });
            throw;
        }
        finally
        {
            if (events.Count > 0)
            {
                await Parallel.ForEachAsync(events, cancellationToken, async (@event, token) => await Bus.PublishAsync(@event, token));
            }
        }
        async Task<IRequest<User>> GetRequestAsync()
        {
            switch (data.Provider?.ToLowerInvariant())
            {
                case null or "":
                    throw new ArgumentNullException(nameof(data));
                case AuthenticationConstant.Provider.Username:
                    return new AuthenticateWithUsernameRequest(data.Username, data.Password);
                case AuthenticationConstant.Provider.Email:
                case AuthenticationConstant.Provider.Phone:
                    throw new NotImplementedException($"The provider '{data.Provider}' is not implemented.");
                case AuthenticationConstant.Provider.Github:
                case AuthenticationConstant.Provider.Google:
                case AuthenticationConstant.Provider.Facebook:
                case AuthenticationConstant.Provider.Microsoft:
                    {
                        var provider = LazyServiceProvider.GetKeyedService<IAuthProvider>(data.Provider);
                        if (provider == null)
                        {
                            throw new NotSupportedException($"The provider '{data.Provider}' is not supported.");
                        }

                        var auth = await provider.AuthorizeAsync(data.Username, cancellationToken);

                        if (auth == null)
                        {
                            throw new InvalidOperationException("Failed to authorize with the external provider.");
                        }

                        return new AuthenticateWithAuthProviderRequest(data.Provider, auth.Id);
                    }

                default:
                    throw new NotSupportedException($"The provider '{data.Provider}' is not supported.");
            }
        }
    }

    /// <inheritdoc />
    public async Task<TokenGrantResultDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        IRequest<User> request = new AuthenticateWithRefreshTokenRequest(refreshToken);

        var events = new List<ApplicationEvent>();
        try
        {
            var user = await Bus.RequestAsync(request, cancellationToken);
            var result = GenerateAccessToken(user);
            events.Add(new UserAuthSucceedEvent
            {
                AuthType = AuthenticationConstant.Provider.RefreshToken,
                RefreshToken = result.RefreshToken,
                UserId = result.UserId,
                Username = result.Username,
                TokenIssueTime = DateTimeHelper.GetDateTimeFromUnixTime(result.IssueAt)
            });

            events.Add(new TokenRefreshedEvent(refreshToken));

            return result;
        }
        finally
        {
            if (events.Count > 0)
            {
                await Parallel.ForEachAsync(events, cancellationToken, async (@event, token) => await Bus.PublishAsync(@event, token));
            }
        }
    }

    /// <inheritdoc />
    public Task RevokeAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private TokenGrantResultDto GenerateAccessToken(User user)
    {
        var roles = user.Roles?.Select(r => r.Name);

        var jti = ObjectId.NewGuid(GuidType.SequentialAsString).ToString("N");

        var issueTime = DateTime.UtcNow;
        var expiresAt = issueTime.AddDays(1);

        var builder = TokenGenerator.Create(user.Id, user.Username)
                                    .WithSigningKey(configuration.GetValue<string>("JwtAuthenticationOptions:SigningKey"))
                                    .WithIssuer(configuration.GetValue<string>("JwtAuthenticationOptions:Issuer:0"))
                                    .AddRole(roles?.ToArray())
                                    .IssuedAt(issueTime)
                                    .AddClaim(JwtClaimTypes.Email, user.Email)
                                    .AddClaim(JwtClaimTypes.PhoneNumber, user.Phone)
                                    .AddClaim(JwtClaimTypes.NickName, user.Nickname);

        var accessToken = builder.Build();

        return new TokenGrantResultDto
        {
            AccessToken = accessToken,
            RefreshToken = ObjectId.NewGuid(GuidType.SequentialAsString).ToString("N"),
            TokenType = AuthenticationConstant.TokenType.Bearer,
            Username = user.Username,
            UserId = user.Id,
            IssueAt = new DateTimeOffset(issueTime).ToUnixTimeSeconds(),
            ExpiresIn = (long)(expiresAt - issueTime).TotalSeconds
        };
    }
}