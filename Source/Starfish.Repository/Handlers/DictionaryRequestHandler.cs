using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines the request handler for dictionary queries
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
		var specification = DictionaryRootSpecification.ApplyCriteria(message.Criteria);
		var predicate = specification.Satisfy();
		return repository.FindAsync(predicate, null, message.Skip, message.Take, cancellationToken)
						 .ContinueWith(task => context.Response(task.Result), cancellationToken);
	}

	public Task HandleAsync(DictionaryRootCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var specification = DictionaryRootSpecification.ApplyCriteria(message.Criteria);
		var predicate = specification.Satisfy();
		return repository.CountAsync(predicate, null, cancellationToken)
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
}