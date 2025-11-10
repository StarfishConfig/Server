using Microsoft.AspNetCore.Authorization;
using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Defines the application service contract for user-related operations.
/// </summary>
public interface IUserApplicationService : IApplicationService
{
    /// <summary>
    /// Retrieves the profile of a user by their username asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<UserProfileDto> GetProfileAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<long> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the phone number of an existing user asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="phone"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdatePhoneAsync(long id, string phone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the email address of an existing user asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateEmailAsync(long id, string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes the password of the current user asynchronously.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ChangePasswordAsync(UserPasswordChangeDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the password of a user asynchronously.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ResetPasswordAsync(UserPasswordResetDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the password of a user by their ID asynchronously.
    /// </summary>
    /// <param name="id">The id of user whose password to be reset.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "SA")]
    Task ResetPasswordAsync(long id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Unlock a user account asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "SA")]
    Task UnlockAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new third-party authority for the current user asynchronously.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateAuthorityAsync(string provider, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a third-party authority from the current user asynchronously.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="openId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAuthorityAsync(string provider, string openId, CancellationToken cancellationToken = default);
}