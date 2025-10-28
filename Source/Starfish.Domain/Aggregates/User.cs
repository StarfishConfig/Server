using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the user information aggregate.
/// </summary>
public sealed class User : Aggregate<long>, IHasCreateTime, IHasUpdateTime, ITombstone
{
    private User()
    {
    }

    /// <summary>
    /// Gets or sets the unique username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password hash stored securely.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the password salt used for hashing.
    /// </summary>
    public string PasswordSalt { get; set; }

    /// <summary>
    /// Gets or sets the nickname to display.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the number of failed access attempts.
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Gets or sets the lockout end time.
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Gets or sets the source of the user account.
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the update time.
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the deletion time.
    /// </summary>
    public DateTime? DeleteTime { get; set; }

    internal static User Create(string username)
    {
        var entity = new User()
        {
            Username = username
        };

        entity.RaiseEvent(new UserCreatedEvent { Username = username });
        return entity;
    }
}