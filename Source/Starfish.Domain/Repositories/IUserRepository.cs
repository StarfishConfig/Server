namespace Nerosoft.Starfish.Domain;

internal interface IUserRepository : IBaseRepository<User, long>
{
    /// <summary>
    /// Finds a user by their username.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="tracking">Give a value to indicate whether the entity should be tracked or not.</param>
    /// <param name="properties"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> FindByUsernameAsync(string username, bool tracking, string[] properties, CancellationToken cancellationToken = default);

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
    
    /// <summary>
    /// Finds a user by an external provider.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="value"></param>
    /// <param name="tracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> FindByProviderAsync(string provider, string value, bool tracking, CancellationToken cancellationToken = default);
}