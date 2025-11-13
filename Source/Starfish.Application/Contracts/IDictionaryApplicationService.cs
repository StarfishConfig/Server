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
}
