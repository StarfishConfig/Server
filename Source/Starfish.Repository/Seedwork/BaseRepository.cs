using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository;
using Nerosoft.Euonia.Repository.EfCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The abstract base repository class connecting to a specific DbContext and managing entities of a specific type with a specific key type.
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class BaseRepository<TContext, TEntity, TKey> : EfCoreRepository<TContext, TEntity, TKey>,
                                                                ITransientDependency
    where TContext : DbContext, IRepositoryContext
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{TContext, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="provider"></param>
    protected BaseRepository(IContextProvider provider)
        : base(provider)
    {
    }

    /// <summary>
    /// Gets the queryable set of a specific entity type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IQueryable<T> SetOf<T>()
        where T : class
    {
        return Context.SetOf<T>();
    }
}