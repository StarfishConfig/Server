using System.Linq.Expressions;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Base repository interface for generic entity operations.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IBaseRepository<TEntity, in TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Inserts a new entity of type <see cref="TEntity"/>.
    /// </summary>
    /// <param name="entity">The new <see cref="TEntity"/> entity instance.</param>
    /// <param name="autoSave">A value indicate whether the context should be saved automatically.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> InsertAsync(TEntity entity, bool autoSave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity of type <see cref="TEntity"/>.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="autoSave">A value indicate whether the context should be saved automatically.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, bool autoSave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its primary ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(TKey id, bool tracking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its primary ID, including specified related properties.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(TKey id, bool tracking, string[] properties, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if any entity exists with the given expression.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of entities matching the given expression.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="handle"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>> handle, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities matching the given predicate with additional query handling.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="handle"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> handle, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities matching the given predicate with additional query handling, supporting pagination.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="handle"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> handle, int offset, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the entity by id.
    /// </summary>
    /// <param name="id">The values of the primary key for the entity to be deleted.</param>
    /// <param name="autoSave">A value to indicate whether the context should save automatically or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <exception cref="NotFoundException"></exception>
    Task DeleteAsync(TKey id, bool autoSave = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity of type <see cref="TEntity"/> and raises a domain event.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="eventFactory"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task DeleteAsync<TEvent>(TEntity entity, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
        where TEvent : DomainEvent;

    /// <summary>
    /// Deletes an entity by its primary ID and raises a domain event.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="eventFactory"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task DeleteAsync<TEvent>(TKey id, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
        where TEvent : DomainEvent;

    /// <summary>
    /// Finds entities with additional query handling.
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> FindAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> handle, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities with additional query handling.
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> handle, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single entity with additional query handling.
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> handle, CancellationToken cancellationToken = default);
}