using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Euonia.Repository.EfCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The abstract base repository class connecting to a specific DbContext and managing entities of a specific type with a specific key type.
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
internal abstract class BaseRepository<TContext, TEntity, TKey> : EfCoreRepository<TContext, TEntity, TKey>,
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

    /// <summary>
    /// Gets a single entity by its ID.
    /// </summary>
    /// <param name="id">The values of the primary key for the entity to be found.</param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual Task<TEntity> GetAsync(TKey id, bool tracking, CancellationToken cancellationToken = default)
    {
        //var lambda = predicate.Compile();
        return GetAsync(id, tracking, Array.Empty<string>(), cancellationToken);
    }

    /// <summary>
    /// Gets a single entity by its ID and includes specified properties.
    /// </summary>
    /// <param name="id">The values of the primary key for the entity to be found.</param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual Task<TEntity> GetAsync(TKey id, bool tracking, string[] properties, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.PropertyEqual<TEntity, TKey>(nameof(IEntity<TKey>.Id), id);

        //var lambda = predicate.Compile();
        return GetAsync(predicate, tracking, properties, cancellationToken);
    }

    /// <summary>
    /// Gets a single entity based on the given predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, CancellationToken cancellationToken = default)
    {
        return GetAsync(predicate, tracking, Array.Empty<string>(), cancellationToken);
    }

    /// <summary>
    /// Gets a single entity based on the given predicate and includes specified properties.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, string[] properties, CancellationToken cancellationToken = default)
    {
        return base.GetAsync(predicate, query => BuildQuery(query, tracking, properties), cancellationToken);
    }

    /// <summary>
    /// Finds entities by their IDs and includes specified properties.
    /// </summary>
    /// <param name="ids">The values of the primary key for the entity to be found.</param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual Task<List<TEntity>> FindAsync(IEnumerable<TKey> ids, string[] properties, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.PropertyInRange<TEntity, TKey>(nameof(IEntity<TKey>.Id), ids.ToArray());
        return FindAsync(predicate, properties, cancellationToken);
    }

    /// <summary>
    /// Finds entities based on the given predicate and includes specified properties.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string[] properties, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(predicate, query => BuildQuery(query, false, properties), cancellationToken);
    }

    /// <summary>
    /// Looks up entities by their IDs and selects a key-value pair using the provided selector.
    /// </summary>
    /// <param name="ids">The values of the primary key for the entity to be found.</param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public virtual async Task<Dictionary<TKey, string>> LookupAsync(IEnumerable<TKey> ids, Expression<Func<TEntity, KeyValuePair<TKey, string>>> selector, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.PropertyInRange<TEntity, TKey>(nameof(IEntity<TKey>.Id), ids.ToArray());
        var query = Context.Set<TEntity>().AsNoTracking()
                           .Where(predicate)
                           .Select(selector);

        var items = await query.ToListAsync(cancellationToken: cancellationToken);

        var result = items.ToDictionary(x => x.Key, x => x.Value);

        return result;
    }

    /// <summary>
    /// Deletes the entity by id.
    /// </summary>
    /// <param name="id">The values of the primary key for the entity to be deleted.</param>
    /// <param name="autoSave">A value to indicate whether the context should save automatically or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <exception cref="NotFoundException"></exception>
    public virtual async Task DeleteAsync(TKey id, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var set = Context.Set<TEntity>();

        var entity = await set.FindAsync([id], cancellationToken: cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException();
        }

        set.Remove(entity);
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Deletes the entity by id and raises a domain event.
    /// </summary>
    /// <param name="id">The values of the primary key for the entity to be deleted.</param>
    /// <param name="eventFactory">The function to create a new domain event instance.</param>
    /// <param name="autoSave">A value to indicate whether the context should save automatically or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <typeparam name="TEvent">The domain event type to raise up.</typeparam>
    /// <exception cref="NotFoundException"></exception>
    public virtual async Task DeleteAsync<TEvent>(TKey id, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
        where TEvent : DomainEvent
    {
        var set = Context.Set<TEntity>();

        var entity = await set.FindAsync([id], cancellationToken: cancellationToken);
        switch (entity)
        {
            case null:
                throw new NotFoundException();
            case IHasDomainEvents aggregate:
            {
                var @event = eventFactory();
                aggregate.RaiseEvent(@event);
                break;
            }
        }

        set.Remove(entity);
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes the entity and raises a domain event.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <param name="eventFactory">The function to create a new domain event instance.</param>
    /// <param name="autoSave">A value to indicate whether the context should save automatically or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <typeparam name="TEvent">The domain event type to raise up.</typeparam>
    /// <returns></returns>
    public virtual Task DeleteAsync<TEvent>(TEntity entity, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
        where TEvent : DomainEvent
    {
        if (entity is IHasDomainEvents aggregate)
        {
            var @event = eventFactory();
            aggregate.RaiseEvent(@event);
        }

        {
        }

        return DeleteAsync(entity, autoSave, cancellationToken);
    }

    /// <summary>
    /// Builds the query with tracking and included properties.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="tracking"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected virtual IQueryable<TEntity> BuildQuery(IQueryable<TEntity> query, bool tracking, string[] properties)
    {
        query = tracking ? query.AsTracking() : query.AsNoTracking();

        if (properties is { Length: > 0 })
        {
            query = properties.Aggregate(query, (current, property) => current.Include(property));
        }

        return query;
    }
}