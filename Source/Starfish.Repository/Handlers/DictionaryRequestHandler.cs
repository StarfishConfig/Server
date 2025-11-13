using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Dictionary request handler
/// </summary>
/// <param name="provider"></param>
internal class DictionaryRequestHandler(ILazyServiceProvider provider)
    : IHandler<DictionaryRootListQueryRequest>,
    IHandler<DictionaryRootCountQueryRequest>,
    IHandler<DictionaryRootDetailQueryRequest>,
    IHandler<DictionaryItemListQueryRequest>,
    IHandler<DictionaryItemCountQueryRequest>,
{
    private readonly IDictionaryRootRepository _rootRepository = provider.GetRequiredService<IDictionaryRootRepository>();
    private readonly IDictionaryItemRepository _itemRepository = provider.GetRequiredService<IDictionaryItemRepository>();

    public Task HandleAsync(DictionaryRootListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return _rootRepository.FindAsync(query => ApplyCriteria(ref query, message.Criteria).Skip(message.Skip).Take(message.Take), cancellationToken)
                              .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryRootCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return _rootRepository.CountAsync(query => ApplyCriteria(ref query, message.Criteria), cancellationToken)
                              .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryRootDetailQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return _rootRepository.GetAsync(message.Id, false, message.Properties, cancellationToken)
                              .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(DictionaryItemListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(DictionaryItemCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

    private static IQueryable<DictionaryItem> ApplyCriteria(ref IQueryable<DictionaryItem> query, long rootId, DictionaryItemCriteriaDto criteria)
    {
        if (criteria != null)
        {
            var specification = DictionaryItemSpecification.RootIdEquals(rootId);
            if (!string.IsNullOrWhiteSpace(criteria.Keyword))
            {
                specification &= DictionaryItemSpecification.Matches(criteria.Keyword);
            }
            var predicate = specification.Satisfy();
            query = query.Where(predicate);
        }
        return query;
    }
}
