using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface IDictionaryApplicationService : IApplicationService
{
    /// <summary>
    /// Gets the list of dictionary roots based on criteria with pagination.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<DictionaryRootListDto>> ListRootAsync(DictionaryRootCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the total number of dictionary roots based on criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountRootAsync(DictionaryRootCriteriaDto criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the detailed information of a dictionary root by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DictionaryRootDetailDto> GetRootDetailAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new dictionary root.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateRootAsync(DictionaryRootCreateDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing dictionary root.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateRootAsync(long id, DictionaryRootUpdateDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a dictionary root by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteRootAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Looks up dictionary entries by their codes.
    /// </summary>
    /// <param name="codes"></param>
    /// <param name="isValid"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<DictionaryLookupDto>> LookupAsync(IEnumerable<string> codes, bool? isValid, CancellationToken cancellationToken = default);

    Task<List<DictionaryItemListDto>> ListItemAsync(long id, DictionaryItemCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

    Task<int> CountItemAsync(long id, DictionaryItemCriteriaDto criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new dictionary item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AppendItemAsync(long id, DictionaryItemCreateDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing dictionary item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateItemAsync(long id, DictionaryItemUpdateDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes dictionary items by their keys.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteItemAsync(long id, IEnumerable<string> keys, CancellationToken cancellationToken = default);
}
