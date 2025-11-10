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

    /// <inheritdoc />
    public async Task HandleAsync(AuthenticateWithUsernameRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.Username))
        {
            throw new ArgumentException(Resources.USERNAME_CAN_NOT_EMPTY, nameof(message.Username));
        }
        if (string.IsNullOrWhiteSpace(message.Password))
        {
            throw new ArgumentException(Resources.PASSWORD_CAN_NOT_EMPTY, nameof(message.Password));
        }

        var user = await UserRepository.FindByUsernameAsync(message.Username, false, cancellationToken);

        if (user == null)
        {
            throw new AuthenticationException(Resources.USERNAME_PASSWORD_INVALID);
        }

        var passwordHash = Cryptography.DES.Encrypt(message.Password, Encoding.UTF8.GetBytes(user.PasswordSalt));

        if (!string.Equals(passwordHash, user.PasswordHash, StringComparison.Ordinal))
        {
            throw new AuthenticationException(Resources.USERNAME_PASSWORD_INVALID);
        }

        if (user.LockoutEnd > DateTime.UtcNow)
        {
            throw new AuthenticationException(Resources.USER_ACCOUNT_LOCKED);
        }

        var (jti, accessToken, refreshToken, issuedAt, expiresAt) = GenerateAccessToken(user);
        var result = new AuthResultDto
        {
            Id = jti,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Username = user.Username,
            UserId = user.Id,
            IssueAt = new DateTimeOffset(issuedAt).ToUnixTimeSeconds(),
            ExpiresIn = (long)(expiresAt - issuedAt).TotalSeconds
        };
        context.Response(result);
    }

    public async Task HandleAsync(AuthenticateWithRefreshTokenRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.RefreshToken))
        {
            throw new ArgumentException(Resources.REFRESH_TOKEN_CAN_NOT_EMPTY, nameof(message.RefreshToken));
        }
    }

    private Tuple<string, string, string, DateTime, DateTime> GenerateAccessToken(User user)
    {
        var jti = Guid.NewGuid().ToString("N");

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
        var refreshToken = Guid.NewGuid().ToString("N");
        return Tuple.Create(jti, accessToken, refreshToken, issueTime, expiresAt);
    }
}
