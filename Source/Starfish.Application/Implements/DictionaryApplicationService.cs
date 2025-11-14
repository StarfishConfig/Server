using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Application service implementation for dictionary management.
/// </summary>
internal class DictionaryApplicationService : BaseApplicationService, IDictionaryApplicationService
{
	/// <summary>
	/// Query dictionary root list based on criteria with pagination.
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<List<DictionaryRootListDto>> ListRootAsync(DictionaryRootCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default)
	{
		return Bus.RequestAsync(new DictionaryRootListQueryRequest(criteria, skip, take), cancellationToken)
		          .ContinueWith(task =>
		          {
			          task.WaitAndUnwrapException(cancellationToken);
			          return TypeAdapter.ProjectedAs<List<DictionaryRootListDto>>(task.Result);
		          }, cancellationToken);
	}

	/// <summary>
	/// Count dictionary root entries based on criteria.
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<int> CountRootAsync(DictionaryRootCriteriaDto criteria, CancellationToken cancellationToken = default)
	{
		return Bus.RequestAsync(new DictionaryRootCountQueryRequest(criteria), cancellationToken);
	}

	/// <summary>
	/// Get detailed information of a dictionary root by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<DictionaryRootDetailDto> GetRootDetailAsync(long id, CancellationToken cancellationToken = default)
	{
		return Bus.RequestAsync(new DictionaryRootDetailQueryRequest(id), cancellationToken)
		          .ContinueWith(task =>
		          {
			          task.WaitAndUnwrapException(cancellationToken);
			          return TypeAdapter.ProjectedAs<DictionaryRootDetailDto>(task.Result);
		          }, cancellationToken);
	}

	/// <summary>
	/// Create a new dictionary root entry.
	/// </summary>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task CreateRootAsync(DictionaryRootCreateDto data, CancellationToken cancellationToken = default)
	{
		var command = TypeAdapter.ProjectedAs<DictionaryRootCreateCommand>(data);
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <summary>
	/// Update an existing dictionary root entry.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task UpdateRootAsync(long id, DictionaryRootUpdateDto data, CancellationToken cancellationToken = default)
	{
		var command = new DictionaryRootUpdateCommand(id);
		TypeAdapter.ProjectedAs(data, command);
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <summary>
	/// Delete a dictionary root entry by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task DeleteRootAsync(long id, CancellationToken cancellationToken = default)
	{
		var command = new DictionaryRootDeleteCommand(id);
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <summary>
	/// Lookup dictionary entries by their codes and validity.
	/// </summary>
	/// <param name="codes"></param>
	/// <param name="isValid"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<List<DictionaryLookupDto>> LookupAsync(IEnumerable<string> codes, bool? isValid, CancellationToken cancellationToken = default)
	{
		return Bus.RequestAsync(new DictionaryLookupQueryRequest(codes, isValid), cancellationToken)
		          .ContinueWith(task =>
		          {
			          task.WaitAndUnwrapException(cancellationToken);

			          return Group(task.Result);
		          }, cancellationToken);

		List<DictionaryLookupDto> Group(List<DictionaryFlattenModel> models)
		{
			var group = from model in models
			            group model by new { model.Code, model.Name }
			            into g
			            select new DictionaryLookupDto
			            {
				            Code = g.Key.Code,
				            Name = g.Key.Name,
				            Items = g.ToDictionary(t => t.Key, t => t.Value)
			            };
			return group.OrderBy(t => t.Code).ToList();
		}
	}

	/// <summary>
	/// List dictionary items under a specific root based on criteria with pagination.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<List<DictionaryItemListDto>> ListItemAsync(long id, DictionaryItemCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default)
	{
		return Bus.RequestAsync(new DictionaryItemListQueryRequest(id, criteria, skip, take), cancellationToken)
		          .ContinueWith(task =>
		          {
			          task.WaitAndUnwrapException(cancellationToken);
			          return TypeAdapter.ProjectedAs<List<DictionaryItemListDto>>(task.Result);
		          }, cancellationToken);
	}

	/// <summary>
	/// Count dictionary items under a specific root based on criteria.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<int> CountItemAsync(long id, DictionaryItemCriteriaDto criteria, CancellationToken cancellationToken = default)
	{
		return Bus.RequestAsync(new DictionaryItemCountQueryRequest(id, criteria), cancellationToken);
	}

	/// <summary>
	/// Append a new dictionary item under a specific root.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task AppendItemAsync(long id, DictionaryItemCreateDto data, CancellationToken cancellationToken = default)
	{
		var command = new DictionaryItemCreateCommand(id);
		TypeAdapter.ProjectedAs(data, command);
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <summary>
	/// Update an existing dictionary item under a specific root.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task UpdateItemAsync(long id, DictionaryItemUpdateDto data, CancellationToken cancellationToken = default)
	{
		var command = new DictionaryItemUpdateCommand(id);
		TypeAdapter.ProjectedAs(data, command);
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <summary>
	/// Delete dictionary items by their keys under a specific root.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="keys"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task DeleteItemAsync(long id, IEnumerable<string> keys, CancellationToken cancellationToken = default)
	{
		var command = new DictionaryItemDeleteCommand(id, keys);
		return Bus.SendAsync(command, cancellationToken);
	}
}