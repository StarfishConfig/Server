using System.Security.Authentication;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Shared;
using Nerosoft.Starfish.Toolkit;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Handles authentication requests.
/// </summary>
/// <param name="provider"></param>
internal class AuthRequestHandler(IServiceProvider provider)
    : IHandler<AuthenticateWithUsernameRequest>,
      IHandler<AuthenticateWithRefreshTokenRequest>,
      IHandler<AuthenticateWithAuthProviderRequest>
{
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

        var user = await UserRepository.FindByUsernameAsync(message.Username, false, [nameof(User.Roles)], cancellationToken);

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

        context.Response(user);
    }

    /// <inheritdoc />
    public async Task HandleAsync(AuthenticateWithRefreshTokenRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.Token))
        {
            throw new ArgumentException(Resources.IDS_ERROR_REFRESH_TOKEN_REQUIRED, nameof(message.Token));
        }

        var key = message.Token.ToSha256();

        var token = await TokenRepository.FindByKeyAsync(key, false, cancellationToken);

        if (token == null)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_REFRESH_TOKEN_INVALID);
        }

        if (token.Expires < DateTime.UtcNow)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_REFRESH_TOKEN_EXPIRED);
        }

        var user = await UserRepository.GetAsync(token.Subject, false, [nameof(User.Roles)], cancellationToken);

        if (user == null)
        {
            throw new AuthenticationException(string.Format(Resources.IDS_ERROR_USER_NOT_FOUND, token.Subject));
        }

        context.Response(user);
    }

    /// <inheritdoc />
    public async Task HandleAsync(AuthenticateWithAuthProviderRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.Provider))
        {
            throw new BadRequestException("The external authentication provider is required.");
        }

        if (string.IsNullOrWhiteSpace(message.OpenId))
        {
            throw new BadRequestException("The external authentication open ID is required.");
        }

        var user = await UserRepository.FindByProviderAsync(message.Provider, message.OpenId, false, cancellationToken);
        if (user == null)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_EXTERNAL_LOGIN_FAILED);
        }

        if (user.LockoutEnd > DateTime.UtcNow)
        {
            throw new AuthenticationException(Resources.IDS_ERROR_USER_LOCKOUT);
        }

        context.Response(user);
    }
}