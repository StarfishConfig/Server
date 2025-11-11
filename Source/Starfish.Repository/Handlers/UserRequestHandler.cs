using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Handles user query requests.
/// </summary>
/// <param name="repository"></param>
internal sealed class UserRequestHandler(IUserRepository repository)
    : IHandler<UserProfileQueryRequest>
{
    /// <inheritdoc />
    public Task HandleAsync(UserProfileQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.GetAsync(message.UserId, false, [], cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }
}