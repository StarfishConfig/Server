using System.Linq.Expressions;

namespace Nerosoft.Starfish.Domain;

public interface IUserRepository
{
    /// <summary>
    /// Inserts a new user entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> InsertAsync(User entity, bool autoSave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(User entity, bool autoSave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> GetAsync(long id, bool tracking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their ID, including specified related properties.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tracking"></param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> GetAsync(long id, bool tracking, string[] properties, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if any user exists with the given expression.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a user by their username.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> FindByUsernameAsync(string username, bool tracking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a username already exists.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CheckUsernameExistsAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email already exists, ignoring a specific user ID.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="ignoreId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CheckEmailExistsAsync(string email, long ignoreId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a phone number already exists, ignoring a specific user ID.
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="ignoreId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CheckPhoneExistsAsync(string phone, long ignoreId, CancellationToken cancellationToken = default);
}