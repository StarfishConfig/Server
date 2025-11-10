using System.Security.Authentication;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Handles authentication requests.
/// </summary>
/// <param name="provider"></param>
internal class AuthRequestHandler(IServiceProvider provider)
    : IHandler<AuthenticateWithUsernameRequest>,
      IHandler<AuthenticateWithRefreshTokenRequest>
{
    private IConfiguration _configuration;
    private IConfiguration Configuration => _configuration ??= provider.GetRequiredService<IConfiguration>();

    private IUserRepository _userRepository;
    private IUserRepository UserRepository => _userRepository ??= provider.GetRequiredService<IUserRepository>();

    private ITokenRepository _tokenRepository;
    private ITokenRepository TokenRepository => _tokenRepository ??= provider.GetRequiredService<ITokenRepository>();

    /// <inheritdoc />
    public async Task HandleAsync(AuthenticateWithUsernameRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.Username))
        {
            throw new ArgumentException(Resources.IDS_ERROR_USERNAME_REQUIRED, nameof(message.Username));
        }
        if (string.IsNullOrWhiteSpace(message.Password))
        {
            throw new ArgumentException(Resources.IDS_ERROR_PASSWORD_REQUIRED, nameof(message.Password));
        }

        var user = await UserRepository.FindByUsernameAsync(message.Username, false, cancellationToken);

        if (user == null)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_USERNAME_PASSWORD_INVALID);
        }

        var passwordHash = Cryptography.DES.Encrypt(message.Password, Encoding.UTF8.GetBytes(user.PasswordSalt));

        if (!string.Equals(passwordHash, user.PasswordHash, StringComparison.Ordinal))
        {
            throw new AuthenticationException(Resources.IDS_ERROR_USERNAME_PASSWORD_INVALID);
        }

        if (user.LockoutEnd > DateTime.UtcNow)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_USER_LOCKOUT);
        }

        var result = GenerateAccessToken(user);
        context.Response(result);
    }

    public async Task HandleAsync(AuthenticateWithRefreshTokenRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.Token))
        {
            throw new ArgumentException(Resources.IDS_ERROR_REFRESH_TOKEN_REQUIRED, nameof(message.Token));
        }

        var key = message.Token.ToSha256();

        var token = await TokenRepository.FindByKeyAsync(key, cancellationToken);

        if (token == null)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_REFRESH_TOKEN_INVALID);
        }

        if (token.Expires < DateTime.UtcNow)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_REFRESH_TOKEN_EXPIRED);
        }

        var user = await UserRepository.GetAsync(token.Subject, false, cancellationToken);

        if (user == null)
        {
            throw new AuthenticationException(string.Format(Resources.IDS_ERROR_USER_NOT_FOUND, token.Subject));
        }

        var result = GenerateAccessToken(user);
        context.Response(result);
    }

    private AuthResultDto GenerateAccessToken(User user)
    {
        var jti = ObjectId.NewGuid(GuidType.SequentialAsString).ToString("N");

        var issueTime = DateTime.UtcNow;
        var expiresAt = issueTime.AddDays(1);

        var builder = TokenGenerator.Create(user.Id, user.Username)
                                    .WithSigningKey(Configuration.GetValue<string>("JwtAuthenticationOptions:SigningKey"))
                                    .WithIssuer(Configuration.GetValue<string>("JwtAuthenticationOptions:Issuer:0"))
                                    //.AddRole(roles?.ToArray())
                                    .IssuedAt(issueTime)
                                    .AddClaim(JwtClaimTypes.Email, user.Email)
                                    .AddClaim(JwtClaimTypes.PhoneNumber, user.Phone)
                                    .AddClaim(JwtClaimTypes.NickName, user.Nickname)
                                    .AddClaim(JwtClaimTypes.JwtId, jti);

        var accessToken = builder.Build();

        return new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = ObjectId.NewGuid(GuidType.SequentialAsString).ToString("N"),
            TokenType = "Bearer",
            Username = user.Username,
            UserId = user.Id,
            IssueAt = new DateTimeOffset(issueTime).ToUnixTimeSeconds(),
            ExpiresIn = (long)(expiresAt - issueTime).TotalSeconds
        };
    }
}
