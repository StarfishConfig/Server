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
    public Task<UserProfileDto> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var request = new UserProfileQueryRequest(User.GetUserIdOfInt64());
        return Bus.RequestAsync(request, cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      return TypeAdapter.ProjectedAs<UserProfileDto>(task.Result);
                  });
    }

    /// <inheritdoc />
    public Task<long> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default)
    {
        var command = TypeAdapter.ProjectedAs<UserCreateCommand>(data);
        command.Source = 2;
        return Bus.SendAsync<UserCreateCommand, long>(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task UpdatePhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        var command = new UserUpdateCommand(User.GetUserIdOfInt64())
        {
            Phone = phone
        };
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task UpdateEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var command = new UserUpdateCommand(User.GetUserIdOfInt64())
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
    public Task ResetPasswordAsync(long id, string password, CancellationToken cancellationToken = default)
    {
        var command = new UserPasswordUpdateCommand(id, password, UserPasswordChangeTypeConstant.Reset);
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task UnlockAsync(long id, CancellationToken cancellationToken = default)
    {
        var command = new UserFailureResetCommand(id);
        return Bus.SendAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CreateAuthorityAsync(string provider, string code, CancellationToken cancellationToken = default)
    {
        var authProvider = LazyServiceProvider.GetKeyedService<IAuthProvider>(provider.ToLowerInvariant());

        if (authProvider == null)
        {
            throw new NotSupportedException(string.Format(Resources.IDS_ERROR_EXTERNAL_PROVIDER_NOT_SUPPORTED, provider));
        }

        var result = await authProvider.AuthorizeAsync(code, cancellationToken);

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