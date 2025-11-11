using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Shared;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Implements the application service for user-related operations.
/// </summary>
internal sealed class UserApplicationService : BaseApplicationService, IUserApplicationService
{
    /// <inheritdoc />
    public ValueTask<UserProfileResultDto> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var request = new UserProfileQueryRequest(User.GetUserIdOfInt64());
        return Bus.RequestAsync(request, cancellationToken).AsValueTask();
    }

    /// <inheritdoc />
    public ValueTask<long> CreateAsync(UserCreateRequestDto data, CancellationToken cancellationToken = default)
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
    public Task ChangePasswordAsync(UserPasswordChangeRequestDto data, CancellationToken cancellationToken = default)
    {
        var command = new UserPasswordUpdateCommand(User.GetUserIdOfInt64(), data.Password, UserPasswordChangeTypeConstant.Change);
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task ResetPasswordAsync(UserPasswordResetRequestDto data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task ResetPasswordAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task UnlockAsync(long id, CancellationToken cancellationToken = default)
    {
        var command = new UserUnlockCommand(id);
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CreateAuthorityAsync(string provider, string code, CancellationToken cancellationToken = default)
    {
        var authProvider = LazyServiceProvider.GetKeyedService<IAuthProvider>(provider.ToLowerInvariant());

        if (authProvider == null)
        {
            throw new NotSupportedException("Provider is not supported.");
        }

        var result = await authProvider.AuthorizeAsync(code, cancellationToken);

        if (result == null)
        {
            throw new BadRequestException("Invalid authorization result.");
        }

        var command = new UserAuthorityCreateCommand(User.GetUserIdOfInt64(), provider, result.Id)
        {
            Name = result.Username ?? result.Nickname
        };
        await Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task RemoveAuthorityAsync(string provider, string openId, CancellationToken cancellationToken = default)
    {
        var command = new UserAuthorityRemoveCommand(User.GetUserIdOfInt64(), provider, openId);
        return Bus.SendAsync(command, cancellationToken);
    }
}