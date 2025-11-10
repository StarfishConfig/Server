using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application.Requests;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

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
                         .ContinueWith(task =>
                         {
                             task.WaitAndUnwrapException(cancellationToken);
                             var result = TypeAdapter.ProjectedAs<UserProfileDto>(task.Result);
                             context.Response(result);
                         }, cancellationToken);
    }
}