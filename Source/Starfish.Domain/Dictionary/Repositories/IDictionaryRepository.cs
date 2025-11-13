using System.Linq.Expressions;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the repository for dictionary roots.
/// </summary>
public interface IDictionaryRepository : IBaseRepository<DictionaryRoot, long>
{
    /// <summary>
    /// Finds dictionary items associated with a specific root ID that match the given predicate.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="predicate"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<DictionaryItem>> FindItemAsync(long id, Expression<Func<DictionaryItem, bool>> predicate, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of dictionary items associated with a specific root ID that match the given predicate.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountItemAsync(long id, Expression<Func<DictionaryItem, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds dictionary items associated with a specific root ID that match the given keyword.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keyword"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<DictionaryItem>> FindItemAsync(long id, string keyword, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of dictionary items associated with a specific root ID that match the given keyword.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keyword"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountItemAsync(long id, string keyword, CancellationToken cancellationToken = default);

    /// <summary>
    /// Flattens dictionary data for the specified codes and validity status.
    /// </summary>
    /// <param name="codes"></param>
    /// <param name="isValid"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<DictionaryFlattenModel>> FlattenAsync(IEnumerable<string> codes, bool? isValid, CancellationToken cancellationToken = default);
}