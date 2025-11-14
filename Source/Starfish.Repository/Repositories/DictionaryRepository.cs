using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Dictionary root repository
/// </summary>
internal class DictionaryRepository : BaseRepository<SystemDataContext, DictionaryRoot, long>, IDictionaryRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryRepository"/> class.
    /// </summary>
    /// <param name="provider"></param>
    public DictionaryRepository(IContextProvider provider)
        : base(provider)
    {
    }

    public Task<List<DictionaryItem>> FindItemAsync(long id, Expression<Func<DictionaryItem, bool>> predicate, int skip, int take, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<DictionaryItem>().AsQueryable();
        query = query.Where(i => i.RootId == id);
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = query.Skip(skip).Take(take);
        return query.ToListAsync(cancellationToken);
    }

    public Task<int> CountItemAsync(long id, Expression<Func<DictionaryItem, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<DictionaryItem>().AsQueryable();
        query = query.Where(i => i.RootId == id);
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.CountAsync(cancellationToken);
    }

    public Task<List<DictionaryItem>> FindItemAsync(long id, string keyword, int skip, int take, CancellationToken cancellationToken = default)
    {
        var specification = DictionaryItemSpecification.RootIdEquals(id);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            specification &= DictionaryItemSpecification.Matches(keyword);
        }

        var predicate = specification.Satisfy();

        return Context.Set<DictionaryItem>()
                      .Where(predicate)
                      .OrderBy(t => t.Id)
                      .Skip(skip).Take(take)
                      .ToListAsync(cancellationToken);
    }

    public Task<int> CountItemAsync(long id, string keyword, CancellationToken cancellationToken = default)
    {
        var specification = DictionaryItemSpecification.RootIdEquals(id);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            specification &= DictionaryItemSpecification.Matches(keyword);
        }

        var predicate = specification.Satisfy();

        return Context.Set<DictionaryItem>()
                      .Where(predicate)
                      .CountAsync(cancellationToken);
    }

    public Task<List<DictionaryFlattenModel>> FlattenAsync(IEnumerable<string> codes, bool? isValid, CancellationToken cancellationToken = default)
    {
        var query = from root in Context.Set<DictionaryRoot>()
                    join item in Context.Set<DictionaryItem>() on root.Id equals item.RootId
                    where codes.Contains(root.Code)
                    select new DictionaryFlattenModel
                    {
                        Id = root.Id,
                        Code = root.Code,
                        Name = root.Name,
                        Key = item.Key,
                        Value = item.Value,
                        IsValid = root.IsValid
                    };

        if (isValid.HasValue)
        {
            query = query.Where(x => x.IsValid == isValid.Value);
        }

        return query.ToListAsync(cancellationToken);
    }
}