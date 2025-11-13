using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Dictionary request handler
/// </summary>
/// <param name="repository"></param>
internal class DictionaryRequestHandler(IDictionaryRepository repository)
    : IHandler<DictionaryRootListQueryRequest>,
      IHandler<DictionaryRootCountQueryRequest>,
      IHandler<DictionaryRootDetailQueryRequest>,
      IHandler<DictionaryItemListQueryRequest>,
      IHandler<DictionaryItemCountQueryRequest>,
      IHandler<DictionaryLookupQueryRequest>
{
    public Task HandleAsync(DictionaryRootListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.FindAsync(query => ApplyCriteria(ref query, message.Criteria).Skip(message.Skip).Take(message.Take), cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryRootCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.CountAsync(query => ApplyCriteria(ref query, message.Criteria), cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryRootDetailQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.GetAsync(message.Id, false, message.Properties, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryItemListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.FindItemAsync(message.RootId, message.Criteria.Keyword, message.Skip, message.Take, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryItemCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.CountItemAsync(message.RootId, message.Criteria.Keyword, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryLookupQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.FlattenAsync(message.Codes, message.IsValid, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    private static IQueryable<DictionaryRoot> ApplyCriteria(ref IQueryable<DictionaryRoot> query, DictionaryRootCriteriaDto criteria)
    {
        if (criteria != null)
        {
            if (!string.IsNullOrWhiteSpace(criteria.Keyword))
            {
                query = query.Where(x => x.Code.Contains(criteria.Keyword) || x.Name.Contains(criteria.Keyword));
            }

            switch (criteria.IsValid)
            {
                case true:
                    query = query.Where(x => x.IsValid);
                    break;
                case false:
                    query = query.Where(x => !x.IsValid);
                    break;
            }
        }

        return query;
    }
}