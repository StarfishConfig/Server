using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application.Requests;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Shared;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Implements the application service for user-related operations.
/// </summary>
internal sealed class UserApplicationService : BaseApplicationService, IUserApplicationService
{
    /// <inheritdoc />
    public ValueTask<UserProfileDto> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var request = new UserProfileQueryRequest(User.GetUserIdOfInt64());
        return Bus.RequestAsync(request, cancellationToken).AsValueTask();
    }

    /// <inheritdoc />
    public ValueTask<long> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default)
    {
        var command = TypeAdapter.ProjectedAs<UserCreateCommand>(data);
        command.Source = 2;
        return Bus.SendAsync<UserCreateCommand, long>(command, cancellationToken).AsValueTask();
    }

    /// <inheritdoc />
    public Task UpdatePhoneAsync(long id, string phone, CancellationToken cancellationToken = default)
    {
        var command = new UserUpdateCommand(id)
        {
            Phone = phone
        };
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task UpdateEmailAsync(long id, string email, CancellationToken cancellationToken = default)
    {
        var command = new UserUpdateCommand(id)
        {
            Email = email
        };
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task ChangePasswordAsync(UserPasswordChangeDto data, CancellationToken cancellationToken = default)
    {
        var command = new UserPasswordUpdateCommand(User.GetUserIdOfInt64(), data.Password, UserPasswordChangeTypeConstant.Change);
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task ResetPasswordAsync(UserPasswordResetDto data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task ResetPasswordAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UnlockAsync(long id, CancellationToken cancellationToken = default)
    {
        var command = new UserUnlockCommand(id);
        return Bus.SendAsync(command, cancellationToken);
    }
}