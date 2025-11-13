using System.Linq.Expressions;
using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Handles user query requests.
/// </summary>
/// <param name="repository"></param>
internal sealed class UserRequestHandler(IUserRepository repository)
    : IHandler<UserProfileQueryRequest>,
      IHandler<UserListQueryRequest>,
      IHandler<UserCountQueryRequest>
{
    /// <inheritdoc />
    public Task HandleAsync(UserProfileQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.GetAsync(message.UserId, false, [], cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(UserListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        var predicate = BuildPredicate(message.Criteria);
        return repository.FindAsync(predicate, null, message.Skip, message.Take, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(UserCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        var predicate = BuildPredicate(message.Criteria);
        return repository.CountAsync(predicate, null, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    private static Expression<Func<User, bool>> BuildPredicate(UserCriteriaDto criteria)
    {
        var specification = UserSpecification.Valid();

        if (criteria.Source > 0)
        {
            specification &= UserSpecification.SourceEquals(criteria.Source!.Value);
        }

        switch (criteria.Locked)
        {
            case true:
                specification &= UserSpecification.IsLockedOut();
                break;
            case false:
                specification &= UserSpecification.IsNotLockedOut();
                break;
        }

        if (!string.IsNullOrWhiteSpace(criteria.Keyword))
        {
            specification &= UserSpecification.Matches(criteria.Keyword);
        }

        return specification.Satisfy();
    }
}